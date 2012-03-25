if (!isObject(CanBeAttackedBehavior))
{
	%template = new BehaviorTemplate(CanBeAttackedBehavior);

	%template.friendlyName = "Is Attacked";
	%template.behaviorType = "Game";
	%template.description  = "Allows an object to be attacked";
	
	%this.hitFilterBehavior = ""; //"isZoneHitFiltered";
}

function CanBeAttackedBehavior::onBehaviorAdd(%this)
{
	
}

function CanBeAttackedBehavior::triggerHit(%this,%initiator,%point,%damage)
{
	// Filter hit?	
	if (%this.hitFilterBehavior !$= "") {
	
	} else {
		// Pass through directly.
		%this.owner.getBehavior("IsMortalBehavior").takeDamage(%damage);
	}
}


