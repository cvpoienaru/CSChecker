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
 ***	any way out of the use of this software, even if advised of	 the possibility of such	***
 ***	damage.																					***
 ***																							***
 **************************************************************************************************
 **************************************************************************************************
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSChecker.Utilities
{
	/// <summary>
	/// Provides methods and properties for managing the output.
	/// </summary>
	internal static class Printer
	{
		#region *** Methods ***
		/// <summary>
		/// Prints the specified character for the specified number of times.
		/// </summary>
		/// 
		/// <param name="character">The character to be printed.</param>
		/// <param name="times">The number of times to print the character.</param>
		/// 
		/// <returns>
		/// A string containing the specified character for the specified number of times.
		/// </returns>
		/// 
		/// <exception cref="System.ArgumentException">
		/// Exception thrown when the specified number of times to print the character is less than zero.
		/// </exception>
		public static string PrintCharacter (char character, int times)
		{
			if (times < 0)
				throw new ArgumentException("The specified number of times is less than zero.");

			StringBuilder stringBuilder = new StringBuilder();

			for (int i = 0; i < times; i++)
				stringBuilder.Append(character);

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