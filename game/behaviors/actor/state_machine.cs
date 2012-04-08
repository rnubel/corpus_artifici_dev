 if (!isObject(StateMachineBehavior))
{
	%template = new BehaviorTemplate(StateMachineBehavior);

	%template.friendlyName = "Obey State Machine";
	%template.behaviorType = "Actor";
	%template.description = "Constrain this actor's actions and movements through a state machine.";
}

function StateMachineBehavior::onBehaviorAdd(%this)
{		
	//%this.baseStateAttributes = [];
	//%this.realStateTransitions = [];
	
	%this.state = "ready_notblocking";
	%this.delayedEventKey = 0;
	%this.direction = "right";
	
	%this.addState("ready", "blockable");
	%this.addState("turning", "blockable");
	%this.addState("walking", "blockable");	
	%this.addState("running", "");
	%this.addState("attacking", "");
	%this.addState("jumping", "");
	
	%this.addTransition("ready", 		"startMoving",		"walking");
	%this.addTransition("walking", 		"stopMoving",		"ready");
	%this.addTransition("ready",		"startTurning",		"turning");
	%this.addTransition("ready",		"startAttacking",	"attacking");
	%this.addTransition("attacking",	"stopAttacking",	"ready");
	%this.addTransition("turning",		"doneTurning",		"ready");
	%this.addTransition("ready",		"startJumping",		"jumping");
	%this.addTransition("walking",		"startJumping",		"jumping");
	%this.addTransition("jumping",		"hitGround",		"ready");
	
	%this.addEntryCallback("walking", 	"%this.startedWalking();");
	%this.addEntryCallback("ready", 	"%this.stoppedMoving();");
	%this.addEntryCallback("turning", 	"%this.startedTurning();");	
	%this.addEntryCallback("attacking", "%this.startedAttacking();");
	%this.addEntryCallback("jumping",	"%this.startedJumping();");
	
	%this.addPreEventCallback("doneTurning",	"%this.doneTurning();");
	
	%this.addPostEventCallback("startBlocking",	"%this.delayEvent(\"doneStartingToBlock\",500,\"\");");
	%this.addPostEventCallback("stopBlocking",	"%this.delayEvent(\"doneStoppingBlocking\",500,\"\");");
}

function StateMachineBehavior::onUpdate(%this)
{
	
}

function StateMachineBehavior::addEntryCallback(%this, %stateName, %eval)
{
	%this.entryCallbacks[%stateName @ "_notblocking"] 		= %eval;
	%this.entryCallbacks[%stateName @ "_startingblocking"] 	= %eval;
	%this.entryCallbacks[%stateName @ "_blocking"] 			= %eval;
	%this.entryCallbacks[%stateName @ "_stoppingblocking"]	= %eval;	
}

function StateMachineBehavior::addPreEventCallback(%this, %eventName, %eval)
{
	%this.preEventCallbacks[%eventName] = %eval;	
}

function StateMachineBehavior::addPostEventCallback(%this, %eventName, %eval)
{
	%this.postEventCallbacks[%eventName] = %eval;	
}

function StateMachineBehavior::addState(%this, %stateName, %attributes)
{
	%this.baseStateAttributes[%stateName] = %attributes;
	%blockable = ( strstr(%attributes, "blockable") >= 0 );
		
	if (%blockable) {
		%this.realStateTransitions[%stateName @ "_notblocking","startBlocking"] 			= %stateName @ "_startingblocking";		
		%this.realStateTransitions[%stateName @ "_startingblocking","doneStartingToBlock"] 	= %stateName @ "_blocking";
		%this.realStateTransitions[%stateName @ "_blocking","stopBlocking"]	 				= %stateName @ "_stoppingblocking";
		%this.realStateTransitions[%stateName @ "_stoppingblocking","doneStoppingBlocking"]	= %stateName @ "_notblocking";		
	}
}

function StateMachineBehavior::stateIsBlockable(%this, %baseStateName) 
{
	return strstr(%this.baseStateAttributes[%baseStateName], "blockable") >= 0;
}

function StateMachineBehavior::addTransition(%this, %baseStateFrom, %event, %baseStateTo)
{
	%this.realStateTransitions[%baseStateFrom @ "_notblocking", %event] = %baseStateTo @ "_notblocking";
	
	if (%this.stateIsBlockable(%baseStateFrom) && %this.stateIsBlockable(%baseStateTo)) {
		%this.realStateTransitions[%baseStateFrom @ "_blocking", %event] = %baseStateTo @ "_blocking";
		%this.realStateTransitions[%baseStateFrom @ "_startingblocking", %event] = %baseStateTo @ "_startingblocking";
		%this.realStateTransitions[%baseStateFrom @ "_stoppingblocking", %event] = %baseStateTo @ "_stoppingblocking";
	}
}

function StateMachineBehavior::reactToEvent(%this, %event) 
{
	%toState = %this.realStateTransitions[%this.state, %event];
	echo("reacting to event " @ %event @ " in state " @ %this.state @ ": " @ %toState);
	
	// Perform event callback.
	eval(%this.preEventCallbacks[%event]);
	
	if (%toState $= "") {
		// Event ignored.
		return false;
	} else {
		echo(%this.state @ " -[" @ %event @ "]-> " @ %toState);
		
		if (strstr(%event, "block") > -1) {
			%this.delayedEventKey += 1; // Invalidate any delayed events.
		}
		
		%this.state = %toState;
		
		// Call any callbacks.
		eval(%this.postEventCallbacks[%event]);
		eval(%this.entryCallbacks[%this.state]);
		
		return true;
	}
}

// Mechanism to delay event transitions assuming our state does not change pre-emptively.
function StateMachineBehavior::delayEvent(%this, %event, %time, %eval) 
{
	//%this.delayedEventKey += 1;
	%this.schedule(%time, "executeDelayedEvent", %event, %this.delayedEventKey, %eval);
}

function StateMachineBehavior::executeDelayedEvent(%this, %event, %key, %eval) 
{
	if (%key == %this.delayedEventKey) {
		echo("Calling delayed event" SPC %event);
		%this.reactToEvent(%event);
		eval(%eval);
	} else {
		echo("Delayed event " @ %event @ " discarded due to preemptive transition.");
	}
}


// **** INTERFACE LAYER (could be pulled out later) ****

function StateMachineBehavior::moveLeft(%this) {
	if (%this.direction $= "left") {
		%this.reactToEvent("startMoving");
	} else if (%this.direction $= "right") {
		if (%this.reactToEvent("startTurning")) {
			%this.delayEvent("doneTurning", 500, "%this.direction = \"left\";");
		}
	}
}

function StateMachineBehavior::moveRight(%this) {
	if (%this.direction $= "right") {
		%this.reactToEvent("startMoving");
	} else if (%this.direction $= "left") {
		if (%this.reactToEvent("startTurning")) {
			%this.delayEvent("doneTurning", 500, "%this.direction = \"right\";");
		}
	}
}

function StateMachineBehavior::block(%this) {
	%this.reactToEvent("startBlocking");
	%this.delayEvent("doneStartingToBlock", 500);
}

function StateMachineBehavior::stopBlocking(%this) {
	%this.reactToEvent("stopBlocking");
	%this.delayEvent("doneStoppingBlocking", 300);
}

function StateMachineBehavior::jump(%this) {
	%this.reactToEvent("startJumping");
}

// **** CALLBACKS ****
function StateMachineBehavior::startedWalking(%this) {
	if (%this.direction $= "left") {
		%this.owner.setLinearVelocityX(-10);
	} else {
		%this.owner.setLinearVelocityX(10);
	}		
	
	%this.updateAnimation();
}

function StateMachineBehavior::stoppedMoving(%this) {
	%this.owner.setLinearVelocityX(0);
	
	%this.updateAnimation();
}

function StateMachineBehavior::startedJumping(%this) {
	%this.owner.setLinearVelocityY(-15);
	
	%this.updateAnimation();
}

function StateMachineBehavior::updateAnimation(%this) {
	if (%this.isWalking()) {
		if (%this.isBlocking()) {
			%this.owner.playAnimation(basicBlockWalk);
		} else {
			%this.owner.playAnimation(basicWalk);
		}
	}
	else if (%this.isAttacking()) {
		%this.owner.playAnimation(basicAttack);
	}
	else {
		if (%this.isBlocking()) {
			%this.owner.playAnimation(basicBlockIdle);
		} else {
			%this.owner.playAnimation(basicIdle);
		}
	}
	
	
}

function StateMachineBehavior::startedTurning(%this) {
	%this.updateAnimation();
}

function StateMachineBehavior::startedAttacking(%this) {
	%this.updateAnimation();
}

function StateMachineBehavior::doneTurning(%this) {
	%this.owner.setFlipX(!%this.owner.getFlipX());
}

function StateMachineBehavior::isBlocking(%this) {
	return (strstr(%this.state, "_blocking") > 0);
}

function StateMachineBehavior::isWalking(%this) {
	return (strstr(%this.state, "walking") == 0);
}

function StateMachineBehavior::isAttacking(%this) {
	return (strstr(%this.state, "attacking") == 0);
}

// FIXME: pull out
function StateMachineBehavior::onCollision(%this, %dstObj, %srcRef, %dstRef, %time, %normal, %contacts, %points)
{   
	 if (getWord(%normal, 1) == -1) {
		%this.reactToEvent("hitGround");
	 } else {
		echo(%normal @ " is no good");
	 }
}