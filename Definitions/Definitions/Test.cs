﻿/**************************************************************************************************
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
		/// The default width of the summary.
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
		/// The current width of the summary.
		/// </summary>
		private int width;
		#endregion *** Fields ***



		#region *** Constructors ***
		/// <summary>
		/// Initializes a new instance of <see cref="CSChecker.Definitions.Test"/> class.
		/// </summary>
		/// 
		/// <param name="description">The description of the current test.</param>
		/// <param name="width">The width of the summary.</param>
		/// 
		/// <exception cref="System.ArgumentException">
		/// Exception thrown when the description argument is null, empty or contains only white spaces or
		/// when the specified width is less than the minimum valid width.
		/// </exception>
		public Test (string description, int width)
		{
			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("Invalid description.");

			if (width < Test.DefaultWidth)
				throw new ArgumentException("The specified width is less than the minimum valid width.");

			this.description = description;
			this.width = width;
			this.subtestCollection = new Queue<Subtest>(0);
		}

		/// <summary>
		/// Initializes a new instance of <see cref="CSChecker.Definitions.Test"/> class.
		/// </summary>
		/// 
		/// <param name="description">The description of the current test.</param>
		public Test (string description)
			: this(description, Test.DefaultWidth)
		{
			// Do nothing.
			// The call of the specified constructor is sufficient in order to get the job done.
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
				throw new ArgumentNullException("Subtest argument is null.");	

			this.subtestCollection.Enqueue(subtest);
		}

		/// <summary>
		/// Runs the collection of subtests, calculating the number of them that passed.
		/// </summary>
		public virtual void Run ()
		{
			for (int i = 0; i < this.subtestCollection.Count; i++)
			{
				// Dequeue a subtest from the collection.
				Subtest subtest = this.subtestCollection.Dequeue();

				// Run the subtest and check its result.
				subtest.Run();
				if (subtest.Result == TestResult.Passed)
					this.passed++;

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
			StringBuilder summary = new StringBuilder();

			// Build the summary for the current test.
			// Add the description of the current test to the output.
			summary.AppendLine(this.description);
			summary.AppendLine(Printer.PrintCharacter('-', this.width));
			for (int i = 0; i < this.subtestCollection.Count; i++)
			{
				// For each subtest, add the specific summary.
				Subtest subtest = this.subtestCollection.Dequeue();
				summary.AppendFormat("\t{0}. {1}", i + 1, subtest.ToString());
				summary.AppendLine();
				this.subtestCollection.Enqueue(subtest);
			}

			// Add the conclusion for the current test (number of passed tests and percentage).
			summary.AppendLine(Printer.PrintCharacter('-', this.width));
			summary.AppendFormat(
				"Passed: {0}/{1} => {2} %",
				this.Passed,
				this.Total,
				Math.Round(((double)(this.Passed * 100)) / this.Total, 2));

			try
			{
				return summary.ToString();
			}
			finally
			{
				summary.Clear();
			}
		}
		#endregion *** Methods ***
	}
}