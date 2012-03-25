if (!isObject(IsMortalBehavior))
{
	%template = new BehaviorTemplate(IsMortalBehavior);

	%template.friendlyName = "Is Mortal";
	%template.behaviorType = "Game";
	%template.description  = "Allows an object to take damage and die";	
}

function IsMortalBehavior::onBehaviorAdd(%this) {
	%this.health = 100;
}


function IsMortalBehavior::takeDamage(%this, %amount) {
	echo("Taking damage of " @ %amount @ "!");
	%this.health -= %amount;
	
	if (%this.health <= 0) {
		%this.die();
	}
}

function IsMortalBehavior::die(%this) {
	%this.owner.removeFromScene();
}
