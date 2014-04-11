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
 ***		* Redistributions of source code must retain the above copyright notice, this list	***
 ***		of conditions and the following disclaimer.											***
 ***																							***
 ***		* Redistributions in binary form must reproduce the above copyright notice, this	***
 ***		list of conditions and the following disclaimer in the documentation and/or other	***
 ***		materials provided with the distribution.											***
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
		/// The default width of the digest.
		/// </summary>
		private const int DefaultWidth = 150;

		/// <summary>
		/// The current width of the digest.
		/// </summary>
		private int width;

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
		/// <param name="width">The width of the digest.</param>
		/// 
		/// <exception cref="System.ArgumentException">
		/// Exception thrown when the description argument is null, empty or contains only white spaces or
		/// when the specified width is less than the minimum valid width.
		/// </exception>
		public Subtest (string description, int width)
		{
			if (string.IsNullOrWhiteSpace(description))
			{
				throw new ArgumentException("Invalid description.");
			}

			// The specified width must be at least equal with the minimum valid width.
			// The minimum valid width is composed by the length of the description and the length of the
			// result, plus 2 characters on each side. The difference between the specified width and the
			// minimum valid width is filled up with dots.
			int validWidth = this.description.Length + this.result.ToString().Length + 2;
			if (width < validWidth)
			{
				throw new ArgumentException("The specified width is less than the minimum valid width.");
			}

			this.description = description;
			this.width = width;
			this.result = TestResult.Failed;
		}

		/// <summary>
		/// Initializes a new instance of CSChecker.Definitions.Subtest class.
		/// </summary>
		/// 
		/// <param name="description">The description of the current subtest.</param>
		public Subtest (string description)
			: this(description, Subtest.DefaultWidth)
		{
			// Do nothing.
			// The call of the specified constructor is sufficient in order to get the job done.
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
		/// Prints the specified number of dots in order to build the digest.
		/// </summary>
		/// 
		/// <param name="n">The number of dots to be printed.</param>
		/// 
		/// <returns>A string containing the specified number of dots.</returns>
		private string PrintDots (int n)
		{
			StringBuilder stringBuilder = new StringBuilder();

			for (int i = 0; i < n; i++)
			{
				stringBuilder.Append('.');
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Provides the string representation of the current subtest.
		/// </summary>
		/// 
		/// <returns>Returns the string representation of the current subtest.</returns>
		public override string ToString ()
		{
			int width = this.width - this.description.Length - this.result.ToString().Length - 2;
			StringBuilder stringBuilder = new StringBuilder();

			// Build the digest for the current subtest.
			stringBuilder.AppendFormat("{0} {1} {2}", this.description, this.PrintDots(width), this.result);

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