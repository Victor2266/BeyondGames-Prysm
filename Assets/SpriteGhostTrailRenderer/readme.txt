- Sprite Ghost Trail Renderer

	This document will explain what is and how to setup property the Sprite Ghost Trail Renderer for your projects.

- What is Sprite Ghost Trail Renderer?

	Sprite Ghost Trail Renderer is a component that creates a ghost trails from your sprite. Ghost trails are clones of sprites drawn on screen with some specific alpha color different than the original sprite. This type of technique was heavily used in 90's games to indicate speed and still used today for the same purpose.
	Ghosts are created once in Start function due performance purposes and you can choose the color, update interval, single color shader and the number of ghosts. They are enabled/disabled as the script is. 

- How it work?

	This component should be attached to any Gameobject that has a SpriteRenderer component. If a SpriteRenderer component is not present, one will attached automatically.
	Ghosts are created once using the Start function due performance purposes and you can choose the color, update interval, single color shader, number of ghosts or if they need to be enable on awake function.

- Setup
Property 			|	Function
Color 					Color of ghosts. The alpha is set here.
Enable on awake			Check it if you want to see the ghosts on awake.
Single color shader 	Check it if you want to see the ghost using single color.
Update Interval 		The time one ghost will wait to draw another.
Ghosts 					The number of ghosts
Sprite Renderer 		The SpriteRenderer component from this gameobject instance.

- Using Sprite Trail Renderer
	Just drag the SpriteTrailRenderer.cs script into any GameObject that has a SpriteRenderer Component. You can setup the properties above or simply press play button.
	If you need more information on how to use it, please see the Example Scene available on Scenes Folder or watch the main tutorial video following the link below:
	https://youtu.be/YlacsBWt45E


Thanks for reading.