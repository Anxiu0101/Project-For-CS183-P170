/*
* Author: XiyanFlowC <1197129380@qq.com>
* Usage: To read data from csv files.
* Date: 2021/6/20
* This file is apart of SMSA.
*/
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace server.Source
{
    /// <summary>
    /// A reader to read csv files
    /// </summary>
    public static class CSVReader
    {
        /// <summary>
        /// Parse csv by lines
        /// </summary>
        /// <param name="lines">Each line of CSV file</param>
        /// <returns>Parsed array of array of string</returns>
        public static string[][] ReadCSV(IEnumerable<string> lines)
        {
            List<string[]> Rows = new List<string[]>();
            foreach (var line in lines)
            {
                int stage = 0;
                List<string> lineBuilder = new List<string>();
                StringBuilder sb = new StringBuilder();
                foreach (char ch in line)
                {
                    switch (stage)
                    {
                        case 0:
                        if(ch == '\"')
                        {
                            stage = 1;// escape
                        }
                        else
                        {
                            sb.Append(ch);
                            stage = 2;// normal
                        }
                        break;
                        case 1:// escaped?
                        if(ch == '\"')
                        {
                            sb.Append('\"');
                            stage = 2;// normal
                        }
                        else
                        {
                            sb.Append(ch);
                            stage = 3;// escaped
                        }
                        break;
                        case 2:
                        if(ch == ',')
                        {
                            lineBuilder.Add(sb.ToString());
                            sb.Clear();
                            stage = 0;
                        }
                        else
                        {
                            sb.Append(ch);
                        }
                        break;
                        case 3:// escaped seq
                        if(ch == '\"')
                        {
                            stage = 4;// escape?
                        }
                        else
                        {
                            sb.Append(ch);
                        }
                        break;
                        case 4:
                        if(ch == '\"')
                        {
                            sb.Append('\"');// escaped
                            stage = 3;
                        }
                        else if(ch == ',')// ended
                        {
                            lineBuilder.Add(sb.ToString());
                            sb.Clear();
                            stage = 0;
                        }
                        else
                        throw new Exception("Format incorrect.");
                        break;
                    }
                }
                if (sb.Length != 0) lineBuilder.Add(sb.ToString());
                Rows.Add(lineBuilder.ToArray());
            }
            return Rows.ToArray();
        }

        public static string[][] ReadCSV(string filepath)
        {
            return ReadCSV(File.ReadLines(filepath));
        }
    }
}