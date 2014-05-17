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
	internal static class MessageDigest
	{
		#region *** Methods ***
		/// <summary>
		/// Appends the specified salt to the specified data input.
		/// For internal use only.
		/// </summary>
		/// 
		/// <param name="data">The data input.</param>
		/// <param name="salt">The salt to be appended to the data input.</param>
		/// 
		/// <returns>
		/// Returns a newly created byte array as a result of appending the specified salt to the
		/// specified data input.
		/// </returns>
		private static byte[] Append (byte[] data, byte[] salt)
		{
			int index;
			int dataLength = data.Length;
			int saltLength = salt.Length;
			byte[] newData = new byte[dataLength + saltLength];

			for (index = 0; index < dataLength + saltLength; index++)
			{
				// Create the desired data output by simply appending the bytes of the salt input
				// at the end of the old data's byte array.
				newData[index] = index < dataLength ? data[index] : salt[index - dataLength];
			}

			// If no failure has occurred by now, the operation is successful.
			return newData;
		}

		/// <summary>
		/// Computes the secure hash for the specified data input.
		/// The size in bites of the resulting digest is equal to the specified data output size.
		/// </summary>
		/// 
		/// <param name="data">The data input.</param>
		/// <param name="OutputSize">
		/// The required data output size for the secure hash standard.
		/// </param>
		/// 
		/// <returns>
		/// Returns a newly created byte array as a result of computing the secure hash for the
		/// specified data input.
		/// </returns>
		/// 
		/// <exception cref="System.ArgumentException">
		/// exception thrown when the data input is null or empty.
		/// </exception>
		public static byte[] Compute (byte[] Data, MessageDigestOutputSize OutputSize)
		{
			if (Data == null)
			{
				// The operation fails because the specified data input is null or empty.
				// Pass an exception to the higher levels of the application in order to take
				// correct action.
				throw new ArgumentException("");
			}

			byte[] Hash = null;

			try
			{
				switch (OutputSize)
				{
					case MessageDigestOutputSize.Bits256:

						using (SHA256Managed Sha = new SHA256Managed())
						{
							// Computes the secure hash for the specified data input.
							// The size in bites of the resulting digest is equal to the specified
							// data output size.
							Hash = Sha.ComputeHash(Data);
						}
						break;

					case MessageDigestOutputSize.Bits384:

						using (SHA384Managed Sha = new SHA384Managed())
						{
							// Computes the secure hash for the specified data input.
							// The size in bites of the resulting digest is equal to the specified
							// data output size.
							Hash = Sha.ComputeHash(Data);
						}
						break;

					case MessageDigestOutputSize.Bits512:

						using (SHA512Managed Sha = new SHA512Managed())
						{
							// Computes the secure hash for the specified data input.
							// The size in bites of the resulting digest is equal to the specified
							// data output size.
							Hash = Sha.ComputeHash(Data);
						}
						break;
				}
			}
			catch (Exception Exception)
			{
				// The operation fails due to an exception occurrence.
				// Pass the exception to the higher levels of the application in order to take
				// correct action.
				throw;
			}

			// If no failure has occurred by now, the operation is successful.
			return Hash;
		}

		/// <summary>
		/// Computes the secure hash for the specified data input.
		/// The size in bites of the resulting digest is equal to the default data output size
		/// (512 bites).
		/// </summary>
		/// 
		/// <param name="data">The data input.</param>
		/// 
		/// <returns>
		/// Returns a newly created byte array as a result of computing the secure hash for the
		/// specified data input.
		/// </returns>
		public static byte[] Compute (byte[] Data)
		{
			return MessageDigest.Compute(Data, MessageDigestOutputSize.Bits512);
		}

		/// <summary>
		/// Appends the specified salt to the specified data input.
		/// Computes the secure hash for the resulted data.
		/// The size in bites of the resulting digest is equal to the specified data output size.
		/// </summary>
		/// 
		/// <param name="data">The data input.</param>
		/// <param name="salt">The salt to be appended to the data input.</param>
		/// <param name="OutputSize">
		/// The required data output size for the secure hash standard.
		/// </param>
		/// 
		/// <returns>
		/// Returns a newly created byte array as a result of computing the secure hash for the
		/// specified data input and the specified salt.
		/// </returns>
		/// 
		/// <exception cref="System.ArgumentException">
		/// exception thrown when one of the arguments is null or empty.
		/// </exception>
		public static byte[] Compute (byte[] Data, byte[] Salt, MessageDigestOutputSize OutputSize)
		{
			if (Data == null)
			{

				// The operation fails because the specified data input is null or empty.
				// Pass an exception to the higher levels of the application in order to take
				// correct action.
				throw new ArgumentException("");
			}

			if (Salt == null)
			{

				// The operation fails because the specified salt input is null or empty.
				// Pass an exception to the higher levels of the application in order to take
				// correct action.
				throw new ArgumentException("");
			}

			// If no failure has occurred by now, the operation is successful.
			return MessageDigest.Compute(MessageDigest.Append(Data, Salt), OutputSize);
		}

		/// <summary>
		/// Appends the specified salt to the specified data input.
		/// Computes the secure hash for the resulted data.
		/// The size in bites of the resulting digest is equal to the default data output size
		/// (512 bites).
		/// </summary>
		/// 
		/// <param name="data">The data input.</param>
		/// <param name="salt">The salt to be appended to the data input.</param>
		/// 
		/// <returns>
		/// Returns a newly created byte array as a result of computing the secure hash for the
		/// specified data input and the specified salt.
		/// </returns>
		public static byte[] Compute (byte[] Data, byte[] Salt)
		{
			return MessageDigest.Compute(Data, Salt, MessageDigestOutputSize.Bits512);
		}

		/// <summary>
		/// Compares the specified secure hash sample with the specified secure hash template.
		/// </summary>
		/// 
		/// <param name="HashSample">The secure hash sample.</param>
		/// <param name="HashTemplate">The secure hash template.</param>
		/// 
		/// <returns>Returns true if the two secure hashes are equal, false otherwise.</returns>
		/// 
		/// <exception cref="System.ArgumentException">
		/// exception thrown when one of the arguments is null or empty.
		/// </exception>
		/// <exception cref="System.InvalidOperationException">
		/// exception thrown when the length of the secure hash sample is not equal to the length
		/// of the secure hash template.
		/// </exception>
		public static bool Compare (byte[] HashSample, byte[] HashTemplate)
		{
			if (HashSample == null)
			{

				// The operation fails because the specified hash sample is null or empty.
				// Pass an exception to the higher levels of the application in order to take
				// correct action.
				throw new ArgumentException("");
			}

			if (HashTemplate == null)
			{

				// The operation fails because the specified hash template is null or empty.
				// Pass an exception to the higher levels of the application in order to take
				// correct action.
				throw new ArgumentException("");
			}

			int Index;
			int HashSampleLength = HashSample.Length;
			int HashTemplateLength = HashTemplate.Length;

			if (HashSampleLength != HashTemplateLength)
			{

				// The operation fails because the length of the hash sample does not match
				// the length of the hash template.
				// Pass an exception to the higher levels of the application in order to take
				// correct action.
				throw new InvalidOperationException("");
			}

			for (Index = 0; Index < HashSampleLength; Index++)
			{
				if (HashSample[Index] != HashTemplate[Index])
				{
					// The operation fails because the current pair of bytes did not match.
					return false;
				}
			}

			// If no failure has occurred by now, the operation is successful.
			return true;
		}
		#endregion *** Methods ***
	}
}