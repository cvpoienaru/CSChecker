/**************************************************************************************************
 **************************************************************************************************
 **************************************************************************************************
 ***																							***
 ***		Author:		Codrin-Victor Poienaru													***
 ***		Email:		cvpoienaru@gmail.com													***
 ***																							***
 ***		Copyright (c) 2014, Codrin-Victor Poienaru.											***
 ***		All rights reserved.																***
 ***																							***
 ***	Redistribution and use in source and binary forms, with or without modification,		***
 ***	are permitted provided that the following conditions are met:							***
 ***																							***
 ***		* Redistributions of source code must retain the above copyright notice, this		***
 ***		list of conditions and the following disclaimer.									***
 ***																							***
 ***		* Redistributions in binary form must reproduce the above copyright notice, this	***
 ***		list of conditions and the following disclaimer in the documentation and/or			***
 ***		other materials provided with the distribution.										***
 ***																							***
 ***	This software is provided by the copyright holders and contributors "as is" and any		***
 ***	express or implied warranties, including, but not limited to, the implied warranties	***
 ***	of merchantability and fitness for a particular purpose are disclaimed. In no event		***
 ***	shall the copyright holder or contributors be liable for any direct, indirect,			***
 ***	incidental, special, exemplary, or consequential damages (including, but not limited	***
 ***	to, procurement of substitute goods or services; loss of use, data, or profits; or		***
 ***	business interruption) however caused and on any theory of liability, whether in		***
 ***	contract, strict liability, or tort (including negligence or otherwise) arising in		***
 ***	any way out of the use of this software, even if advised of the possibility of such		***
 ***	damage.																					***
 ***																							***
 **************************************************************************************************
 **************************************************************************************************
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSChecker.Utilities;

namespace CSChecker.Definitions
{
	/// <summary>
	/// Provides methods and properties for managing an unit.
	/// This class offers support for adding units to be evaluated by the checker.
	/// </summary>
	public abstract class Unit
	{
		#region *** Fields ***
		/// <summary>
		/// The default width of the summary.
		/// </summary>
		private const int DefaultWidth = 80;

		/// <summary>
		/// A watch used to measure the temporal complexity of the current unit.
		/// </summary>
		private Stopwatch watch;

		/// <summary>
		/// A collection of all the tests composing the current unit.
		/// </summary>
		private Queue<Test> testCollection;

		/// <summary>
		/// The description of the current unit.
		/// </summary>
		private string description;

		/// <summary>
		/// The total number of subtests passed when running the current unit.
		/// </summary>
		private int passed;

		/// <summary>
		/// The total number of subtests that the current unit has.
		/// </summary>
		private int total;

		/// <summary>
		/// The current width of the summary.
		/// </summary>
		private int width;
		#endregion *** Fields ***



		#region *** Constructors ***
		/// <summary>
		/// Initializes a new instance of <see cref="CSChecker.Definitions.Unit"/> class.
		/// </summary>
		/// 
		/// <param name="description">The description of the current unit.</param>
		/// <param name="width">The width of the summary.</param>
		public Unit (string description, int width)
		{
			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("Invalid description.");

			if (width < Unit.DefaultWidth)
				throw new ArgumentException("The specified width is less than the minimum valid width.");

			this.description = description;
			this.width = width;
			this.testCollection = new Queue<Test>(0);
		}

		/// <summary>
		/// Initializes a new instance of <see cref="CSChecker.Definitions.Unit"/> class.
		/// </summary>
		/// 
		/// <param name="description">The description of the current unit.</param>
		public Unit (string description)
			: this(description, Unit.DefaultWidth)
		{
			// Do nothing.
			// The call of the specified constructor is sufficient in order to get the job done.
		}
		#endregion *** Constructors ***



		#region *** Properties ***
		/// <summary>
		/// Gets the description of the current unit.
		/// </summary>
		public string Description
		{
			get { return this.description; }
		}
		#endregion *** Properties ***



		#region *** Methods ***
		/// <summary>
		/// Adds a new test to the collection.
		/// </summary>
		/// 
		/// <param name="test">The test to be added to the collection.</param>
		/// 
		/// <exception cref="System.ArgumentNullException">
		/// Exception thrown when the test argument is null.
		/// </exception>
		protected void AddTest (Test test)
		{
			if (test == null)
				throw new ArgumentNullException("Test argument is null.");

			this.testCollection.Enqueue(test);
		}

		/// <summary>
		/// Runs the collection of tests, calculating the number of them that passed as well as the
		/// temporal complexity of the current unit.
		/// </summary>
		public virtual void Run ()
		{
			this.watch = new Stopwatch();

			this.watch.Start();
			for (int i = 0; i < this.testCollection.Count; i++)
			{
				// Dequeue a test from the collection.
				Test test = this.testCollection.Dequeue();

				// Run the test and update the status of the current unit.
				test.Run();
				this.passed += test.Passed;
				this.total += test.Total;

				// Enqueue it back in the collection.
				this.testCollection.Enqueue(test);
			}
			this.watch.Stop();
		}

		/// <summary>
		/// Provides the string representation of the current unit.
		/// </summary>
		/// 
		/// <returns>Returns the string representation of the current unit.</returns>
		public override string ToString ()
		{
			StringBuilder summary = new StringBuilder();
			StringBuilder temp = new StringBuilder();
			byte[] digest;

			// Create the header of the summary. The header is composed of the name of the current unit
			// in capitals along with the date and time when the current unit was launched into execution.
			temp.AppendFormat("{0} ({1})", this.description.ToUpper(), DateTime.Now.ToString());
			summary.AppendLine(Printer.PrintCharacter('=', this.width));
			summary.AppendFormat(
				"{0}{1}",
				Printer.PrintCharacter(' ', (this.width - temp.ToString().Length) / 2),
				temp.ToString());
			summary.AppendLine();
			summary.AppendLine(Printer.PrintCharacter('=', this.width));

			for (int i = 0; i < this.testCollection.Count; i++)
			{
				// For each test, add the specific summary.
				Test test = this.testCollection.Dequeue();
				summary.AppendFormat("Test #{0} - {1}", i + 1, test.ToString());
				summary.AppendLine();
				summary.AppendLine(Printer.PrintCharacter('=', this.width));
				this.testCollection.Enqueue(test);
			}

			// Add the conclusion for the current unit:
			// - Average execution time;
			// - Total execution time;
			// - Number of passed tests and percentage;
			summary.AppendFormat(
				"Average Execution Time : {0} ms",
				Math.Round(((double)this.watch.ElapsedMilliseconds) / this.testCollection.Count, 2));
			summary.AppendLine();
			summary.AppendFormat(
				"Total Execution Time   : {0} ms",
				this.watch.ElapsedMilliseconds);
			summary.AppendLine();
			summary.AppendFormat(
				"Total Passed           : {0}/{1} => {2} %",
				this.passed,
				this.total,
				Math.Round(((double)(this.passed * 100)) / this.total, 2));
			summary.AppendLine();
			summary.AppendLine(Printer.PrintCharacter('=', this.width));

			// Compute the digest of the current unit.
			digest = MessageDigest.Compute(
						Encoding.UTF8.GetBytes(summary.ToString()),
						MessageDigestOutputSize.Bits256);

			// Convert the digest into hex format.
			temp.Clear();
			foreach (byte b in digest)
			{
				temp.AppendFormat("{0:x2}", b);
			}

			// Append the digest of the current unit.
			summary.AppendFormat("UNIT DIGEST: {0}", temp.ToString());
			summary.AppendLine();
			summary.Append(Printer.PrintCharacter('=', this.width));

			try
			{
				return summary.ToString();
			}
			finally
			{
				temp.Clear();
				summary.Clear();
			}
		}
		#endregion *** Methods ***
	}
}