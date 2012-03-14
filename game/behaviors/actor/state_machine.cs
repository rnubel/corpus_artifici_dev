 if (!isObject(StateMachineBehavior))
{
	%template = new BehaviorTemplate(StateMachineBehavior);

	%template.friendlyName = "Obey State Machine";
	%template.behaviorType = "Actor";
	%template.description = "Constrain this actor's actions and movements through a state machine.";
}

function StateMachineBehavior::onBehaviorAdd(%this)
{
	%this.state = "ready";
	%this.states = "";	
	%this.stateTransitions = "";
}

function StateMachineBehavior::onUpdate(%this)
{
	
}

function StateMachineBehavior::addState(%this, %stateName, %args)
{
	%this.states[%stateName] = %args;
}

function StateMachineBehavior::addTransition(%this,%transitionName, %stateFrom, %stateTo)
{
	%this.stateTransitions[%stateFrom,%stateTo] = %transitionName;
}