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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSChecker.Definitions
{
	/// <summary>
	/// Provides methods and properties for managing a subtest.
	/// This class offers support for adding subtests to be evaluated by the checker.
	/// </summary>
	public abstract class Subtest
	{
		#region *** Fields ***
		/// <summary>
		/// The description of the current subtest.
		/// </summary>
		private string description;

		/// <summary>
		/// The result of the current subtest (Passed or Failed).
		/// </summary>
		private TestResult result;
		#endregion *** Fields ***



		#region *** Constructors ***
		/// <summary>
		/// Initializes a new instance of CSChecker.Definitions.Subtest class.
		/// </summary>
		/// 
		/// <param name="description">The description of the current subtest.</param>
		/// 
		/// <exception cref="System.ArgumentException">
		/// Exception thrown when the description argument is null, empty or contains only white spaces.
		public Subtest (string description)
		{
			if (string.IsNullOrWhiteSpace(description))
			{
				throw new ArgumentException("Invalid description.");
			}

			this.description = description;
			this.result = TestResult.Failed;
		}
		#endregion *** Constructors ***



		#region *** Properties ***
		/// <summary>
		/// Gets the result of the current subtest (Passed or Failed).
		/// </summary>
		public TestResult Result
		{
			get { return this.result; }

			protected set
			{
				this.result = value;
			}
		}
		#endregion *** Properties ***



		#region *** Methods ***
		/// <summary>
		/// Runs the current subtest modifying its result.
		/// </summary>
		public abstract void Run ();

		/// <summary>
		/// Provides the string representation of the current subtest.
		/// </summary>
		/// 
		/// <returns>Returns the string representation of the current subtest.</returns>
		public override string ToString ()
		{
			StringBuilder stringBuilder = new StringBuilder();

			// Build the digest for the current subtest.
			stringBuilder.AppendFormat("[{0}] -> {1}", this.result, this.description);

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