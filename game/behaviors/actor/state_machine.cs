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
	
	%this.addState("ready", "blockable");
	%this.addState("walking", "blockable");
	%this.addState("running", "");
	
	%this.addTransition("ready", 		"startMoving",		"walking");
	%this.addTransition("walking", 		"stopMoving",		"ready");
}

function StateMachineBehavior::onUpdate(%this)
{
	
}

function StateMachineBehavior::addState(%this, %stateName, %attributes)
{
	%this.baseStateAttributes[%stateName] = %attributes;
	%blockable = ( strstr(%attributes, "blockable") >= 0 );
		
	/*
		Build actual transitions for this state. Rules:
		
		- Actual state becomes four states:
			STATE_blocking
			STATE_startingblocking
			STATE_notblocking
			STATE_stopping_blocking
		- If STATE is blockable:
			- Add transitions:
				- STATE_blocking -[stopBlocking]-> STATE_notblocking			
				- STATE_notblocking -[startBlocking]-> STATE_blocking
		- Later transitions to this state from FROMSTATE are mapped as follows:			
			- If STATE and FROMSTATE are blockable:
				- FROMSTATE_blocking -> STATE_blocking
				- FROMSTATE_notblocking -> STATE_notblocking
			- Else:
				- FROMSTATE_notblocking -> STATE_notblocking
		- Later transitions from this state to TOSTATE are mapped as follows:
			- If STATE and TOSTATE are blockable:
				- STATE_blocking -> TOSTATE_blocking
				- STATE_notblocking -> TOSTATE_notblocking
			- Else:
				- STATE_notblocking -> TOSTATE_notblocking
	*/
	
	
	
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
	if (%toState $= "") {
		// Event ignored.
	} else {
		echo(%this.state @ " -[" @ %event @ "]-> " @ %toState);
		
		%this.delayedEventKey += 1; // Invalidate any delayed events.
		%this.state = %toState;
	}
}

// Mechanism to delay event transitions assuming our state does not change pre-emptively.
function StateMachineBehavior::delayEvent(%this, %event, %time) 
{
	%this.delayedEventKey += 1;
	%this.schedule(%time, "executeDelayedEvent", %event, %this.delayedEventKey);
}

function StateMachineBehavior::executeDelayedEvent(%this, %event, %key) 
{
	if (%key == %this.delayedEventKey) {
		echo("Calling delayed event" SPC %event);
		%this.reactToEvent(%event);
	} else {
		echo("Delayed event discarded due to preemptive transition.");
	}
}


