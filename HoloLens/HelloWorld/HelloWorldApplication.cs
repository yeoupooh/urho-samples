﻿using Urho;
using Urho.Actions;
using Urho.Holographics;
using Urho.Shapes;

namespace HelloWorld
{
	public class HelloWorldApplication : HoloApplication
	{
		Node earthNode;

		public HelloWorldApplication(string pak, bool emulator) : base(pak, emulator) { }

		protected override async void Start()
		{
			// base.Start() creates a basic scene, see
			// https://github.com/xamarin/urho/blob/master/Bindings/Portable/Holographics/HoloApplication.cs#L98-L155
			base.Start();

			// Create a node for the Earth
			earthNode = Scene.CreateChild();
			earthNode.Position = new Vector3(0, 0, 1); //one meter away
			earthNode.SetScale(0.2f); //20cm
			earthNode.Rotation = new Quaternion(x: 0, y: 23.26f, z: 0); // Earth's obliquity is 23°26′

			// Create a Sphere component which is basically 
			// a StaticModel with CoreData\Models\Sphere.mdl model
			var earth = earthNode.CreateComponent<Sphere>();
			// Usually materials are defined via XML files and 
			// should be accessed via ResourceCache.GetMaterial(), but 
			// for our simple case we can use Material.FromImage method
			earth.SetMaterial(Material.FromImage("Earth.jpg"));

			// Same for the Moon
			var moonNode = earthNode.CreateChild();
			moonNode.SetScale(0.4f);
			moonNode.Position = new Vector3(1f, 0, 0);
			var moon = moonNode.CreateComponent<Sphere>();
			moon.SetMaterial(Material.FromImage("Moon.jpg"));

			// Run a few actions to spin the Earth and the Moon
			earthNode.RunActions(new RepeatForever(
				new RotateBy(duration: 1f, deltaAngleX: 0, deltaAngleY: -10, deltaAngleZ: 0)));
			moonNode.RunActions( new RepeatForever(
				new RotateAroundBy(1f, earthNode.WorldPosition, 0, -10, 0)));
		}

		protected override void OnUpdate(float timeStep)
		{
			//For HL optical stabilization (optional)
			FocusWorldPoint = earthNode.WorldPosition;
		}
	}
}