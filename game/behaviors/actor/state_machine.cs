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
	
	%this.addTransition("ready", 		"startMoving",		"walking");
	%this.addTransition("walking", 		"stopMoving",		"ready");
	%this.addTransition("ready",		"startTurning",		"turning");
	%this.addTransition("turning",		"doneTurning",		"ready");
	
	%this.addEntryCallback("walking", 	"%this.startedWalking();");
	%this.addEntryCallback("ready", 	"%this.stoppedMoving();");
	%this.addEntryCallback("turning", 	"%this.startedTurning();");
	
	%this.addEventCallback("doneTurning",	"%this.doneTurning();");
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

function StateMachineBehavior::addEventCallback(%this, %eventName, %eval)
{
	%this.eventCallbacks[%eventName] = %eval;	
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
	return strstr(%this.baseStateAttributes[%baseStateName], "blockable");
}

function StateMachineBehavior::addTransition(%this, %baseStateFrom, %event, %baseStateTo)
{
	%this.realStateTransitions[%baseStateFrom @ "_notblocking", %event] = %baseStateTo @ "_notblocking";
	
	if (%this.stateIsBlockable(%baseStateFrom) && %this.stateIsBlockable(%baseStateTo)) {
		%this.realStateTransitions[%baseStateFrom @ "_blocking", %event] = %baseStateTo @ "_blocking";
	}
}

function StateMachineBehavior::reactToEvent(%this, %event) 
{
	%toState = %this.realStateTransitions[%this.state, %event];
	echo("reacting to event " @ %event @ " in state " @ %this.state @ ": " @ %toState);
	
	// Perform event callback.
	eval(%this.eventCallbacks[%event]);
	
	if (%toState $= "") {
		// Event ignored.
		return false;
	} else {
		echo(%this.state @ " -[" @ %event @ "]-> " @ %toState);
		
		%this.delayedEventKey += 1; // Invalidate any delayed events.
		%this.state = %toState;
		
		// Call any callbacks.
		eval(%this.entryCallbacks[%this.state]);
		
		return true;
	}
}

// Mechanism to delay event transitions assuming our state does not change pre-emptively.
function StateMachineBehavior::delayEvent(%this, %event, %time, %eval) 
{
	%this.delayedEventKey += 1;
	%this.schedule(%time, "executeDelayedEvent", %event, %this.delayedEventKey, %eval);
}

function StateMachineBehavior::executeDelayedEvent(%this, %event, %key, %eval) 
{
	if (%key == %this.delayedEventKey) {
		echo("Calling delayed event" SPC %event);
		%this.reactToEvent(%event);
		eval(%eval);
	} else {
		echo("Delayed event discarded due to preemptive transition.");
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

// **** CALLBACKS ****
function StateMachineBehavior::startedWalking(%this) {
	if (%this.direction $= "left") {
		%this.owner.setLinearVelocityX(-10);
	} else {
		%this.owner.setLinearVelocityX(10);
	}
	%this.owner.playAnimation(basicWalk);
}

function StateMachineBehavior::stoppedMoving(%this) {
	%this.owner.setLinearVelocityX(0);
	%this.owner.playAnimation(basicIdle);
}

function StateMachineBehavior::startedTurning(%this) {
	
}

function StateMachineBehavior::doneTurning(%this) {
	%this.owner.setFlipX(!%this.owner.getFlipX());
}

