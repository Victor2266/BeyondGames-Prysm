Welcome to GhostSprites!


This component, attached to any Gameobject that has a 
SpriteRenderer, will create Ghost trails - this allows 
Sprites that are running their neutral animations idle ghosts,
as well.  The Ghosts are created using an Algorithm(which can be overridden)
that by default fades the opacity of the sprites in the Trail to zero,
evenly distrubuted over the Trail Size.  This means that the larger the trailsize,
the smaller the fluctuation of opacity from one Ghost to the next.  

Let's go over the settings in the GhostSprites Editor window: 
	
	--Trail Size (int), [Range(2, 200)]
	{
		This setting determines the number of frames that 
		will be active for the GameObect's Ghost Trail.
		For Example, a setting of one creates one Ghost that will
		follow your GameObject's previous frame, while a setting of 
		60 will have 60 Ghosts following your GameObject's location
		at the previus 60 frames of animiation.  Alter to preference.
	}
	
	--Spacing  (int), [Range(0, 20)]
	{
		This setting determines how often the GhostSprite's 
		Update algorithm will be processed - a setting of 
		zero will cause the Ghosts to be placed precisely
		at the location your GameObect's Sprite was at each 
		frame of the Trail Size, while a setting of 
		5 will wait 5 frames until the next Ghost will be 
		placed and removed from the Trail via the Algorithm.
		see Tutorial #2 for more infomation and 
		examples about this setting.  Alter to prefernce.
	}
	
	--Color (Color)
	{
		This sets the color that will be used on the GhostSprites in
		the trail.  A Setting of 255,255,255, {Opacity Value}
		will produce a duplicate color of the Sprite used in your gameObject,
		while changing this will alter the hue of the Ghosts in the trail
		(provided that your Sprite is not too dark).  
		
		The most important part of this setting is the Alpha of the color;
		when not using the Color Aplha Override, this determines the initial
		Opacity value of your Ghosts, which is evenly divided among the Ghosts
		in your trail, progressively to a value of zero (transparent) for the 
		last Sprite in the trail.  Suggested setting is 50 to see it in action -
		alter to preference.
	}
	
	--Ghost Material (List<Material>)
	{
		Like the Color setting, Materials in this list will be divided evenly 
		among the Ghosts in your trail.  If you don't provide any Materials,
		then the material from your GameObect's SpriteRenderer will be used
		for the ghosts in the trail.
	}
	
	--Ghost Sprite Sorting Order (int)
	{
		This functions just like the Sorting Order for your SpriteRenderer -
		only for the Ghosts in the trail.  Typically, you want this set 
		to a value that is LESS than the Order In Layer setting in your
		SpriteRenderer - otherwise, a ghost may appear to be "on top of"
		your GameObect.  Alter to preference.  
	}
	
	--Render On Motion (bool)
	
		This allows you to only create Ghosts when the GameObject is in motion,
		and remove Ghosts when not in motion.
		
		This bit requires your GameObject to have a RigidBody2D attached to it 
		to determine when your GameObject is not in motion.  While this is 
		not required for GhostSprites to function, it is recommended highly.
		
	--Color Alpha Override 			   (bool) 
			Alpha Fluctuation Override (float), [Range(0,1)] 
			Alpha Fluctuation Divisor  (int), 	[Range(0,250)]
			
		When the Color Alpha Override bit is set to true,
		it uses the Alpha Fluctuation Override value 
		instead of the Alpha on the selected Color as the initial 
		opacity of the Ghosts in the trail.  This, paired with the 
		Alpha fluctuation Divisor (which provides the amount of 
		Alpha that will be lost from Spite to Sprite, computed by 
		AlphaFluctuationOverride / AlphaFluctuationDivisor) allows
		you to override the default algorithm for calculating how the opacity
		fades over the lifetime of the Ghost Trail.
		Alter to preference, if in use.
	
	--Restore Defaults Button
		
		Restores initial settings to GhostSprites component, as described below:
		
			--Clears the Materials
			--Sets Trail size to 20 Ghosts
			--Resets the Color (new Color (255,255,255,50))
			--Sets Spacing to zero
			--Sets GhostSpriteSortingOrder to zero
			--Sets RenderOnMotion to false
			--Sets ColorAlphaOverride to false
			--Sets AlphaFluctuationOverride to zero
			--Sets AlphaFluctuationDivisor to one
	 
	

Public Methods:  

	public void ClearTrail() // Clears your lists, destorying all Ghosts.  Sets your Trail Size to two 
	
	public void AddToMaterialList(Material material) // Adds a Material to the GhostMaterials list
	
	public void RestoreDefaults() // Restores the defaults settings, as described in the "Restore Defaults Button" functionality, described above
	

Public Members:

	--ZAnchor (int) -1, or 1
		Set to 1 when main camera's position.z is greater than GameObject's that GhostSprites is attached to, and -1 when it isn't.
		This is set initially in the Start() method of GhostSprites - if you alter the camera's Z position to be behind
		the GameObject, set this accordingly to either -1 or 1.  Values that deviate from this will be ignored when set.
		
	--GhostSpriteSortingOrder 
		As described in previous section.
		
	--RenderOnMotion
		As described in previous section.
		
	--TrailSize
		As described in previous section.
		
	--Spacing
		As described in previous section.
		
-------------------------------------------------------------------

There are a few tutororial videos that you can watch on this, 
but if you would like to dive right into it, just drag the 
GhostSprites.cs script onto any GameObject that has a 
SpriteRenderer Component, and you're good to go!

Keep in mind that if you want Ghost like effects, you'll 
need to set the alpha value to a lower setting in the color
picker on the GhostSprites component.  If you would like 
more information on how to rock with this component,
please watch some tutorials!

https://www.youtube.com/watch?v=exwExUHX4YA (Demo)

https://www.youtube.com/watch?v=NWLVsCeSYBw (Tutorial #1)

https://www.youtube.com/watch?v=xx5VYiIlunU (Tutorial #2)

Happy Ghosting!