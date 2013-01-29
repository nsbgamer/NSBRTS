using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crypto
{
    class Encryption
    {
        #region Members

        private string setpkey;
        private string setskey;

        #endregion

        public Encryption()
        {
            setpkey = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 0123456789.,<>?/\\#~@:;[]{}-_=+()*&^%$\"!\'"; 
            setskey = "aR\"P$N^L*J(H=F-D{B[z:x~v\\t?r<p!.8l6j4 h2f0d\'YTWVUXeSgQiOkMmKoIqGsEuCwAy@;#]/}>_,+9)7&5%31nZcb";
        }

        #region Accessors

        public string Setpkey { get { return setpkey; } }
        public string Setskey { get { return setskey; } }

        #endregion

        #region Methods

        public int Checksum(string _str)
        {
            if (_str == "" || _str == null || _str.Length == 0) return -1;

            int totSum = 0;
            char[] strArray = _str.ToCharArray();

            for (int i = 0; i < strArray.Length; i++)
                totSum = totSum + Convert.ToInt32(strArray[i]);

            return totSum;
        }

        public int CheckIsIn(string _str)
        {
            if (_str == "" || Checksum(_str) <= 45) 
                return -1;

	        int isIn = 0;

            char[] strArray = _str.ToCharArray();

            for (int i = 0; i <= _str.Length-1; i++) 
            {
		        if (setpkey.Split(strArray[i]).Length > 0 && strArray[i] != '|')
                    isIn = 1;
	        }

	        if (isIn == 0)
		        return -1;
	        else
		        return 1;
        }

        public string Decrypt(string _strData, int _shuffle)
        {
            int totSum = CheckIsIn(_strData);

            //Console.WriteLine("CHECKSUM.. totSum = " + totSum);

	        if (_strData == "" || _strData == null || totSum == -1) return _strData;

	        double half;
	        string output, temp, pKey2, sKey2;

            Console.WriteLine("CRPYTO: Decrypting Data.. \"" + _strData + "\"");
	
	        output = _strData;
	        temp = _strData;


	        for (int j = 0; j <= 2; j++)
            {
                #region BEGIN KEY CYCLE
		
		        temp = "";
		        pKey2 = setpkey;
		        sKey2 = setskey;

                char[] outputArray = output.ToCharArray();
		        int shuflen;
		        
                if ( 14*_shuffle + 14*(outputArray.Length-1) > pKey2.Length )
			        shuflen = (int)((14*_shuffle + 14*(outputArray.Length-1)) - pKey2.Length*Math.Floor((14.0*_shuffle + 14*(outputArray.Length-1)) / (pKey2.Length)));
		        else
			        shuflen = (int)(14*_shuffle + 14*(outputArray.Length-1));

                //Console.WriteLine("Decrypt error trap: j = " + j + " shuflen = " + shuflen);

		        pKey2 = pKey2.Substring(shuflen, pKey2.Length-shuflen) + pKey2.Substring(0, shuflen);

                //Console.WriteLine("Decrypt error trap: j = " + j + " pKey2 = " + pKey2);

		        int loc = 0;

		        for (int i = outputArray.Length-1; i >= 0; i--)
                {
                    char[] pKey2Array = pKey2.ToCharArray();

                    if ((loc = sKey2.IndexOf(outputArray[i])) != -1)
                    {
				        if (loc >= 0 && loc < pKey2Array.Length)
				            temp += pKey2Array[loc];

                        //Console.WriteLine();
                        //Console.WriteLine("Decrypt error trap: temp = " + temp);
				        
                        pKey2 = pKey2.Substring(pKey2.Length-14, 14) + pKey2.Substring(0, 14) + pKey2.Substring(14, pKey2.Length-(14*2));
                        //Console.WriteLine("Decrypt error trap: i = " + i + " j = " + j + " pKey2 = " + pKey2);
			        }
		        }

		        temp = ReverseString(temp);
		        output = temp;
//                Console.WriteLine();
//                Console.WriteLine("Decrypt error trap: END OF KEY CYCLE temp   = " + temp);
//                Console.WriteLine("Decrypt error trap: END OF KEY CYCLE output = " + output);

                #endregion

                #region BEGIN SWAP ADJACENT CHARS

                char[] tempArray = output.ToCharArray();
                outputArray = output.ToCharArray();

		        for (int i = 0; i < outputArray.Length; i = i + 2) 
                {
			        if (i + 1 < tempArray.Length && i + 1 < outputArray.Length && i >= 0) 
                    {
                        //Console.WriteLine("Decrypt error trap: stopped in IF-statement ***** i         = " + i);
                        tempArray[i] = outputArray[i + 1]; //swap from 2nd to 1st
                        tempArray[i + 1] = outputArray[i]; //swap from 1st to 2nd
                        //Console.Write("Decrypt error trap: stopped in IF-statement ***** tempArray = ");
                        //Console.WriteLine(tempArray);
			        }
		        }

		        output = new String(tempArray); //finalize changes
                temp = new String(tempArray);

                //Console.WriteLine();
                //Console.WriteLine("Decrypt error trap: END OF SWAP ADJACENT CHARS output = " + output);
                //Console.WriteLine("Decrypt error trap: END OF SWAP ADJACENT CHARS temp   = " + temp);
                //Console.WriteLine();

                #endregion

                #region BEGIN SWAP ALTERNATE CHARS

                //Console.WriteLine("Decrypt error trap: BEGIN SWAP ALTERNATE CHARS output = " + output);
                //Console.WriteLine();

                if ((output.Length / 2) - Math.Floor(output.Length / 2.0) == 0.5) 
                    half = Math.Floor(output.Length / 2.0) + 1; 
                else 
                    half = Math.Floor(output.Length / 2.0); //half way of string (round)
		
	            //tempArray is already set
                //now reset outputArray to current contents of output
                outputArray = output.ToCharArray();
                tempArray = output.ToCharArray();

                //Console.Write("Decrypt error trap: BEGIN SWAP ALTERNATE CHARS outputArray = ");
                //Console.WriteLine(outputArray);
                //Console.Write("Decrypt error trap: BEGIN SWAP ALTERNATE CHARS tempArray   = ");
                //Console.WriteLine(tempArray);

                for (int i = 0; i < half; i = i+2) 
                {
				    if (i <= tempArray.Length-1 && i <= outputArray.Length-1 && i >= 0 && outputArray.Length-1-i >= 0) 
                    {
                        tempArray[i] = outputArray[output.Length - 1 - i]; //swap end = begin
                        tempArray[output.Length - 1 - i] = outputArray[i]; //swap begin = end
                        //Console.WriteLine("Decrypt error trap: BEGIN SWAP ALTERNATE CHARS i           = " + i); 
                        //Console.Write("Decrypt error trap: BEGIN SWAP ALTERNATE CHARS tempArray   = ");
                        //Console.WriteLine(tempArray);

				    }
			    }

			    output = new String(tempArray); //finalize changes
                //Console.Write("Decrypt error trap: END   SWAP ALTERNATE CHARS tempArray   = ");
                //Console.WriteLine(tempArray);
                //Console.WriteLine("Decrypt error trap: END SWAP ALTERNATE CHARS   output      = " + output);
                //Console.WriteLine();
                #endregion

	        }//end for j loop

            //Console.WriteLine("Decrypt error trap: END OF SWAP ALTERNATE CHARS output = " + output);
	
	        return output;
        }

        public string Encrypt(string _str, int _num)
        {
            if (_str == "" || _str == null || _str.Length == 0)//.empty()) 
                return _str;

	        double half;
	        string output, temp, pKey2, sKey2;
            char [] tempArray, outputArray;

            Console.WriteLine("CRYPTO: Encrypting Data.. \"" + _str.ToString() + "\"");
	
	        output = _str.ToString();
	        temp = _str.ToString();

        	for (int j = 0; j <= 2; j++)
            {

                tempArray = output.ToCharArray();
                outputArray = output.ToCharArray();

                #region BEGIN SWAP ALTERNATE CHARS

		        if ((outputArray.Length / 2)-Math.Floor(outputArray.Length / 2.0) == 0.5) 
                    half = Math.Floor(outputArray.Length / 2.0) + 1; 
                else 
                    half = Math.Floor(outputArray.Length / 2.0); //half way of string (round)

//                Console.WriteLine("Encrypt error trap: BEGIN SWAP ALTERNATE CHARS half = " + half);


                for (int i = 0; i < half; i = i+2) 
                {
                    tempArray[i] = outputArray[outputArray.Length-1-i]; //swap end = begin
                    tempArray[outputArray.Length-1-i] = outputArray[i]; //swap begin = end
			    }

			    output = new String(tempArray); //finalize changes
//                Console.WriteLine("Encrypt error trap: END OF SWAP ALTERNATE CHARS output = " + output);

//                Console.Write("Encrypt error trap: END OF SWAP ALTERNATE CHARS tempArray = ");
//                Console.WriteLine(tempArray);

                #endregion

                #region BEGIN SWAP ADJACENT CHARS
		        
                outputArray = output.ToCharArray();
                tempArray = output.ToCharArray();

		        for (int i = 0; i < outputArray.Length-1; i = i+2) 
                {
			        tempArray[i] = outputArray[i+1]; //swap from 2nd to 1st
                    tempArray[i+1] = outputArray[i]; //swap from 1st to 2nd
		        }

                output = new String(tempArray); //finalize changes
//                Console.WriteLine("Encrypt error trap: END OF SWAP ADJACENT CHARS output = " + output);

                #endregion

                #region BEGIN KEY CYCLE
		
		        temp = "";
		        pKey2 = setpkey;
		        sKey2 = setskey;
		        int shuflen;
                int loc = 0;

//                Console.WriteLine("Encrypt error trap: INITIALIZE pKey2         = " + pKey2);
//                Console.WriteLine("Encrypt error trap: INITIALIZE sKey2         = " + sKey2);
//                Console.WriteLine();

		        if (14*_num > pKey2.Length)
			        shuflen = (int)((14*_num)-pKey2.Length*Math.Floor((14.0*_num)/(pKey2.Length)));
		        else
			        shuflen = (int)(14*_num);

//                Console.WriteLine("Encrypt error trap: INITIALIZE shuflen = " + shuflen);

		        pKey2 = pKey2.Substring(shuflen, pKey2.Length-(shuflen)) + pKey2.Substring(0, shuflen);

                //create a new copy of the outputArray to make it easier to handle
                outputArray = output.ToCharArray();
                char[] sKey2Array = sKey2.ToCharArray();

//                Console.WriteLine("Encrypt error trap: SHUFFLE W SHUFFLEN pKey2 = " + pKey2);
//                Console.Write("Encrypt error trap: SHUFFLE sKey2Array       = ");
//                Console.WriteLine(sKey2Array);
//                Console.WriteLine("Encrypt error trap: BEGIN KEY CYCLE outputArray.Length = " + outputArray.Length);

		        //for the entire length of output
                for (int i = 0; i < outputArray.Length; i++) 
                {
//                    Console.WriteLine("Encrypt error trap: BEGIN KEY CYCLE outputArray[" + i + "] = " + outputArray[i]);
                    
                    //if the current character in output exists in pKey2
                    if ((loc = pKey2.IndexOf(outputArray[i])) != -1) 
                    {
                        //make sure that loc is bounded by [0,sKey2Array.Length)
				        if (loc >= 0 && loc < sKey2Array.Length)
                            temp += sKey2Array[loc]; //add the correct character to the temp string

				        //shift pKey2 to the left by 14 characters
                        pKey2 = pKey2.Substring(14, pKey2.Length - (14)) + pKey2.Substring(0, 14);


//                        Console.WriteLine("Encrypt error trap: BEGIN KEY CYCLE sKey2Array.Length = " + sKey2Array.Length);
//                        Console.WriteLine("Encrypt error trap: BEGIN KEY CYCLE loc = " + loc);
//                        Console.WriteLine("Encrypt error trap: BEGIN KEY CYCLE loc != 0, stopped in first IF-statement");
//                        Console.WriteLine("Encrypt error trap: BEGIN KEY CYCLE temp = " + temp);
			        } 
                    //otherwise, set loc equal to the location of the '*' in pKey2
                    else if ((loc = pKey2.IndexOf('*', 0)) != -1)
                    {
                        //make sure that loc is bounded by [0,sKey2Array.Length)
                        if (loc >= 0 && loc < sKey2Array.Length)
                            temp += sKey2Array[loc]; //add the correct character to the temp string

                        //shift pKey2 to the left by 14 characters
                        pKey2 = pKey2.Substring(14, pKey2.Length - (14)) + pKey2.Substring(0, 14);
                        
//                        Console.WriteLine("Encrypt error trap: BEGIN KEY CYCLE loc is '*', stopped in ELSE IF-statement");
//                        Console.WriteLine("Encrypt error trap: BEGIN KEY CYCLE temp = " + temp);
			        } 
                    //last case - default
                    else 
                    {
                        //add an 'a' to the end of the temp string
				        temp += 'a';

                        //shift pKey2 to the left by 14 characters
				        pKey2 = pKey2.Substring(14, pKey2.Length-(14)) + pKey2.Substring(0, 14);

//                        Console.WriteLine("Encrypt error trap: BEGIN KEY CYCLE loc char doesn't exist, stopped in ELSE statement");
//                        Console.WriteLine("Encrypt error trap: BEGIN KEY CYCLE temp = " + temp);

                    }
//                    Console.WriteLine("Encrypt error trap: i = " + i + " pKey2 = " + pKey2);
		        }

                //finalize the output string
		        output = temp;
//                Console.WriteLine();
//                Console.WriteLine("Encrypt error trap: END OF FOR J LOOP ************* j = " + j);
//                Console.WriteLine("Encrypt error trap: END OF FOR J LOOP ************* temp = " + temp);
//                Console.WriteLine("Encrypt error trap: END OF FOR J LOOP ************* output = " + output);
//                Console.WriteLine();

                #endregion
            }

            //Console.WriteLine();
            //Console.WriteLine("Encrypt error trap: ENCRYPT ************* FINAL output = " + output);
            //Console.WriteLine();
            //Console.WriteLine();
	
	        return output;

        }


        /// <summary>
        /// Receives string and returns the string with its letters reversed.
        /// </summary>
        public string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public string SwapCharacters(string value, int position1, int position2)
        {
            //
            // Swaps characters in a string. Must copy the characters and reallocate the string.
            //
            char[] array = value.ToCharArray(); // Get characters
            char temp = array[position1]; // Get temporary copy of character
            array[position1] = array[position2]; // Assign element
            array[position2] = temp; // Assign element
            return new string(array); // Return string
        }

        #endregion
    }
}
