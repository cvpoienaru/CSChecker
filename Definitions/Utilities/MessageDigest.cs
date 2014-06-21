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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSChecker.Utilities
{
	/// <summary>
	/// Provides static methods for using message digest standard.
	/// </summary>
	internal static class MessageDigest
	{
		#region *** Methods ***
		/// <summary>
		/// Computes the digest for the specified input.
		/// The size in bites of the resulting digest is equal to the specified output size.
		/// </summary>
		/// 
		/// <param name="data">The data input.</param>
		/// <param name="outputSize">The required output size for the digest.</param>
		/// 
		/// <returns>
		/// Returns a newly created byte array as a result of computing the message digest for the
		/// specified data input.
		/// </returns>
		/// 
		/// <exception cref="System.ArgumentException">
		/// Exception thrown when the data input is null or empty.
		/// </exception>
		public static byte[] Compute (byte[] data, MessageDigestOutputSize outputSize)
		{
			if (data == null || data.Length == 0)
				throw new ArgumentException("Data input is null or empty.");

			byte[] digest = null;

			switch (outputSize)
			{
				case MessageDigestOutputSize.Bits256:
					using (SHA256Managed sha = new SHA256Managed())
					{
						digest = sha.ComputeHash(data);
					}
					break;

				case MessageDigestOutputSize.Bits384:
					using (SHA384Managed sha = new SHA384Managed())
					{
						digest = sha.ComputeHash(data);
					}
					break;

				case MessageDigestOutputSize.Bits512:
					using (SHA512Managed sha = new SHA512Managed())
					{
						digest = sha.ComputeHash(data);
					}
					break;
			}

			return digest;
		}
		#endregion *** Methods ***
	}
}