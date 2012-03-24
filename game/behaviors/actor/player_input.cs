if (!isObject(PlayerInputBehavior))
{
	%template = new BehaviorTemplate(PlayerInputBehavior);

	%template.friendlyName = "Drive State Machine by Player Input";
	%template.behaviorType = "Actor";
	%template.description = "Drive state machine via keyboard and mouse input.";
}


function PlayerInputBehavior::onBehaviorAdd(%this)
{	
	moveMap.bindObj(getWord("keyboard right", 0), 	getWord("keyboard right", 1), 	"setRight", %this);
	moveMap.bindObj(getWord("keyboard left", 0), 	getWord("keyboard left", 1), 	"setLeft", %this);
	//moveMap.bindObj(getWord("mouse0 button1", 0), 	getWord("mouse0 button1", 1), 	"setBlock", %this);
	moveMap.bindObj(getWord("keyboard space", 0), 	getWord("keyboard space", 1), 	"hitAttack", %this);
	
	%this.left = false;
	%this.right = false;
	
	%this.owner.enableUpdateCallback();
	sceneWindow2D.setUseObjectMouseEvents(true);  
	%this.owner.setUseMouseEvents(true);	
}

function PlayerInputBehavior::onLevelLoaded(%this) {
	
	%this.owner.setMouseLocked(true); 
}


function PlayerInputBehavior::onRightMouseDown(%this, %modifier, %worldPosition, %mouseClicks)  
{  	
	%this.setBlock(1);
}  

function PlayerInputBehavior::onRightMouseUp(%this, %modifier, %worldPosition, %mouseClicks)  
{  
	%this.setBlock(0);
}  


function PlayerInputBehavior::setRight(%this, %val)
{	
	%this.right = %val;
	%this.updateMovement();
}

function PlayerInputBehavior::setLeft(%this, %val)
{
	%this.left = %val;
	%this.updateMovement();
}

function PlayerInputBehavior::setBlock(%this, %val)
{
	if (%val) {
		%this.getStateMachine().reactToEvent("startBlocking");
	} else {
		%this.getStateMachine().reactToEvent("stopBlocking");
	}
}

function PlayerInputBehavior::updateMovement(%this) {
	if (%this.left && !%this.right) {
		%this.getStateMachine().moveLeft();
	} else if (%this.right && !%this.left) {
		%this.getStateMachine().moveRight();
	} else if (strstr(%this.getStateMachine().state, "ready") < 0){
		%this.getStateMachine().reactToEvent("stopMoving");
	}
	
	%this.oldLeft = %this.left;
	%this.oldRight = %this.right;
}

function PlayerInputBehavior::onUpdate(%this)
{
	%this.updateMovement();
}

function PlayerInputBehavior::getStateMachine(%this)
{
	return %this.owner.getBehavior("StateMachineBehavior");
}
