internal class TemplateUnit : Unit
{
	#region *** Test ***
	private class TemplateTest : Test
	{
		#region *** Subtest ***
		private class TemplateSubtest : Subtest
		{
			public TemplateSubtest (string name)
				: base(name)
			{
				// TODO: Add initialization logic, if needed.
			}

			public override void Run ()
			{
				// TODO: Add subtest logic.
			}
		}
		#endregion *** Subtest ***

		public TemplateTest (string name)
			: base(name)
		{
			// TODO: Add initialization logic, if needed.
		}

		public override void Run ()
		{
			// TODO: Add test logic.

			// Warning: Don't remove the following line !
			// The checker will stop working if you do so.
			base.Run();
		}
	}
	#endregion *** Test ***

	public TemplateUnit (string name)
		: base(name)
	{
		// TODO: Add initialization logic, if needed.
	}

	public override void Run ()
	{
		// TODO: Add unit logic.

		// Warning: Don't remove the following line !
		// The checker will stop working if you do so.
		base.Run();
	}
}