
function masterGUI::setup(%this)
{
	//Player Vitals
	%this.playerHealth = 100;
	%this.maxPlayerHealth = 100;
	%this.playerStamina = 100;
	%this.maxPlayerStamina = 100;
	
	//Player Info
	%this.playerName = "Cthulhu";
	%this.playerLevel = 7;
	
	//GUI Display Variables
	%this.maskHeight = 50;
	%this.healthBarLength = 200;
	%this.staminaBarLength = 200;
	%this.textGUIML = "<color:FFFFFF><shadow:1:1/>";
	
	%this.setPlayerName(%this.playerName);
	%this.setPlayerLevel(%this.playerLevel);
		
}

function masterGUI::setPlayerHealth(%this,%num)
{
	if (%num < 0)
	{
		echo("Warning: Health set to invalid value!");
	}
	
	%this.playerHealth = %num;
	%this.refreshPlayerHealth();
}

function masterGUI::setPlayerStamina(%this,%num)
{
	if (%num < 0)
	{
		echo("Warning: Stamina set to invalid value!");
	}
	
	%this.playerStamina = %num;
	%this.refreshPlayerStamina();
}

function masterGUI::setMaxPlayerHealth(%this,%num)
{
	if (%num < 0)
	{
		echo("Warning: Stamina set to invalid value!");
	}
	%this.playerHealth = Math.min(%num,%this.playerHealth);
	%this.maxPlayerHealth = %num;

}

function masterGUI::refreshPlayerHealth(%this)
{
   healthbar_top_mask.setExtent(%this.healthBarLength * %this.playerHealth / %this.maxPlayerHealth,%this.maskHeight);   
}


function masterGUI::refreshPlayerStamina(%this)
{
   healthbar_top_mask.setExtent(%this.staminaBarLength * %this.playerStamina / %this.maxPlayerStamina,%this.maskHeight);   
}

function masterGUI::setPlayerName(%this,%name)
{
	player_name_guiml.setText(%this.textGUIML @ %name);
}

function masterGUI::setPlayerLevel(%this,%level)
{
	player_level_guiml.setText(%this.textGUIML @ "Level " @ %level);
}

