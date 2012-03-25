if (!isObject(CanAttackBehavior))
{
	%template = new BehaviorTemplate(CanAttackBehavior);

	%template.friendlyName = "Can Attack";
	%template.behaviorType = "Actor";
	%template.description  = "Allows an object to attack";
}

function CanAttackBehavior::onBehaviorAdd(%this)
{
	// create a green debug line  
	%this.targetLine = DebugLine::createLine("0 1 0");  // color = "R G B [A]" (alpha is optional, 1 is assumed)  
	
	%this.attackPoint = %this.owner.getPosition();
}

function CanAttackBehavior::attack(%this)
{
	%this.isAttacking = true;
	%this.objectHitList = new SimSet();
	%this.i = 0;
	%this.attackPoint = %this.owner.getPosition();
	%stateMachineBehavior = %this.owner.getBehavior("StateMachineBehavior");
	%stateMachineBehavior.reactToEvent("startAttacking");
	
	%stateMachineBehavior.delayEvent("stopAttacking",500, "");	
}

function CanAttackBehavior::stopAttacking(%this)
{
	%this.isAttacking = false;
	%stateMachineBehavior = %this.owner.getBehavior("StateMachineBehavior");
	%stateMachineBehavior.reactToEvent("doneAttacking");
	//%this.objectHitList.listObjects();
}

function CanAttackBehavior::onUpdate(%this)
{
	%this.i += 0.25;
    %this.attackPoint = t2dVectorAdd(%this.owner.getPosition(),t2dVectorScale(%this.i SPC 0,(%this.owner.getFlipX() ? -1 : 1)));
	// have the line draw from this position to some target object's position  
	if (%this.isAttacking)
	{
		//%this.targetLine.draw(%this.owner.getPosition(), %this.attackPoint);		
		%items = %this.owner.scenegraph.pickPoint(%this.attackPoint, bits("1"), -1);
		if (%items !$= "")
		{
		   %itemCount = getWordCount(%items);      
			
		   for (%i = 0; %i < %itemCount; %i++)    
		   {    			  				
			  %str = getWord(%items, %i);			  
			  if (!%this.objectHitList.isMember(%str))
			  {
				%this.objectHitList.add(%str);
				%str.getBehavior("CanBeAttackedBehavior").triggerHit(%this.owner,$this.attackPoint,40);
			  }
		   }		   
		   
		}
	} else {
		%this.targetLine.draw("0 0", "0 0");		
	}
}

// return a zero length vertical line of the specified color  
function DebugLine::createLine(%color)  
{  
   %line = new t2dShapeVector()  
   {  
	  scenegraph = sceneWindow2D.getSceneGraph();  
	  class = "DebugLine";  
	  size = "200 200";  
   };  
	 
   %line.setPolyCustom(2, "0 0 0 0");  
   %line.setLineColor(%color);  
	 
   return %line;  
}  
  
// draw this line from startPoint to endPoint  
function DebugLine::draw(%this, %startPoint, %endPoint)  
{  
   %this.setPosition(%startPoint);  
  // echo("0 0" SPC %this.getLocalPoint(%endPoint));
   %this.setPolyCustom(2, "0 0" SPC %this.getLocalPoint(%endPoint));  
}  