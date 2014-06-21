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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSChecker
{
	/// <summary>
	/// Provides methods and properties for managing the output.
	/// </summary>
	public class Printer
	{
		/// <summary>
		/// Prints the current summary to the specified file.
		/// </summary>
		/// 
		/// <param name="summary">The summary to be printed.</param>
		/// <param name="fileName">The name of the file where to print the summary.</param>
		/// 
		/// <exception cref="System.ArgumentException">
		/// Exception thrown when the summary argument or file name argument is null, empty or contains only
		/// white spaces.
		/// </exception>
		public static void PrintSummary (string summary, string fileName)
		{
			if (string.IsNullOrWhiteSpace(summary))
				throw new ArgumentException("Invalid summary.");

			if (string.IsNullOrWhiteSpace(fileName))
				throw new ArgumentException("Invalid file name.");

			using (StreamWriter streamWriter = new StreamWriter(fileName, false))
			{
				streamWriter.Write(summary);
				streamWriter.Close();
			}
		}
	}
}