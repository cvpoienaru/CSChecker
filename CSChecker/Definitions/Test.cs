using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSChecker.Utilities;

namespace CSChecker.Definitions
{
	/// <summary>
	/// Provides methods and properties for managing a test.
	/// This class offers support for adding tests to be evaluated by the checker.
	/// </summary>
	public abstract class Test
	{
		#region *** Fields ***
		/// <summary>
		/// 
		/// </summary>
		private const int DefaultWidth = 80;

		/// <summary>
		/// A collection of all the subtests composing the current test.
		/// </summary>
		private Queue<Subtest> subtestCollection;

		/// <summary>
		/// The description of the current test.
		/// </summary>
		private string description;

		/// <summary>
		/// The number of subtests passed when running the current test.
		/// </summary>
		private int passed;

		/// <summary>
		/// 
		/// </summary>
		private int width;
		#endregion *** Fields ***



		#region *** Constructors ***
		public Test (string description, int width)
		{
			if (string.IsNullOrWhiteSpace(description))
			{
				throw new ArgumentException("Invalid description.");
			}

			this.description = description;
			this.subtestCollection = new Queue<Subtest>(0);
		}

		/// <summary>
		/// Initializes a new instance of CSChecker.Definitions.Test class.
		/// </summary>
		/// 
		/// <param name="description">The description of the current test.</param>
		/// 
		/// <exception cref="System.ArgumentException">
		/// Exception thrown when the description argument is null, empty or contains only white spaces.
		/// </exception>
		/// 
		public Test (string description)
			: this(description, Test.DefaultWidth)
		{
			
		}
		#endregion *** Constructors ***



		#region *** Properties ***
		/// <summary>
		/// Gets the number of subtests passed when running the current test.
		/// </summary>
		public int Passed
		{
			get { return this.passed; }
		}

		/// <summary>
		/// Gets the total number of subtests composing the current test.
		/// </summary>
		public int Total
		{
			get { return this.subtestCollection.Count; }
		}
		#endregion *** Properties ***



		#region *** Methods ***
		/// <summary>
		/// Adds a new subtest to the collection.
		/// </summary>
		/// 
		/// <param name="subtest">The subtest to be added to the collection.</param>
		/// 
		/// <exception cref="System.ArgumentNullException">
		/// Exception thrown when the subtest argument is null.
		/// </exception>
		protected void AddSubtest (Subtest subtest)
		{
			if (subtest == null)
			{
				throw new ArgumentNullException("Subtest argument is null.");	
			}

			this.subtestCollection.Enqueue(subtest);
		}

		/// <summary>
		/// Runs the collection of subtests, calculating the number of them that passed.
		/// </summary>
		public virtual void Run ()
		{
			for (int i = 0; i < this.subtestCollection.Count; i++)
			{
				// Dequeue a subtest from the collection and run it.
				Subtest subtest = this.subtestCollection.Dequeue();
				subtest.Run();

				if (subtest.Result == TestResult.Passed)
				{
					this.passed++;
				}

				// Enqueue it back in the collection.
				this.subtestCollection.Enqueue(subtest);
			}
		}

		/// <summary>
		/// Provides the string representation of the current test.
		/// </summary>
		/// 
		/// <returns>Returns the string representation of the current test.</returns>
		public override string ToString ()
		{
			StringBuilder stringBuilder = new StringBuilder();

			// Build the digest for the current test.
			// Add the description of the current test to the output.
			stringBuilder.AppendLine(this.description);
			stringBuilder.AppendLine(Printer.PrintCharacter('-', this.width));
			for (int i = 0; i < this.subtestCollection.Count; i++)
			{
				// For each subtest, add the specific digest.
				Subtest subtest = this.subtestCollection.Dequeue();
				stringBuilder.AppendFormat("\t{0}. {1}", i + 1, subtest.ToString());
				stringBuilder.AppendLine();
				this.subtestCollection.Enqueue(subtest);
			}

			// Add the conclusion for the current test (number of passed tests and percentage).
			stringBuilder.AppendLine(Printer.PrintCharacter('-', this.width));
			stringBuilder.AppendFormat(
				"Passed: {0}/{1} => {2} %",
				this.Passed,
				this.Total,
				Math.Round(((double)(this.Passed * 100)) / this.Total, 2));
			stringBuilder.AppendLine();

			try
			{
				return stringBuilder.ToString();
			}
			finally
			{
				stringBuilder.Clear();
			}
		}
		#endregion *** Methods ***
	}
}