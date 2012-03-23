 if (!isObject(CameraFollowsBehavior))
{
	%template = new BehaviorTemplate(CameraFollowsBehavior);

	%template.friendlyName = "Camera Follows";
	%template.behaviorType = "Camera";
	%template.description  = "Camera follows this object.";
}

function CameraFollowsBehavior::onLevelLoaded(%this, %scene)
{
	sceneWindow2d.mount(%this.owner, "0 0", 20, true);
}