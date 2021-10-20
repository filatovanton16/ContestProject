using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContestProject
{
    public class JDoodlePOSTObject
    {
        public string clientId = "2dc5dafe92194a5629efb80069d583a1";   //Put to the Config
        public string clientSecret = "b5804f1b2a42df4a99afe3774e32d11fe59be944596b168ed9947e186514fb1";
        public string stdin;
        public string language = "java";
        public string versionIndex = "3";
        public string script;
        public JDoodlePOSTObject(string _script, int _stdin)
        {
            stdin = _stdin.ToString();
            script = _script.Replace("\n", "").Replace("\r", "");
            script = Helper.ReduceSpaces(script);
        }
    }

    public static class JDoodleConnector
    {

        const string outerLeftTemplate = @"import java.util.Scanner;
                                            public class MyClass { 
                                            public static void main(String args[]) {
                                            Scanner in = new Scanner(System.in);
                                            int input = in.nextInt();
                                            in.close();
		                                    System.out.println(MyMethod(input));
	                                        }";
        const string outerRightTemplate = @" }";
        const string uri = "https://api.jdoodle.com/v1/execute";

        public async static Task<bool> TryCompilate(UserTaskCode userTaskCode)
        {
            JDoodlePOSTObject jdPOSTObject = new JDoodlePOSTObject(outerLeftTemplate + userTaskCode.Code + outerRightTemplate, userTaskCode.InputParameter);

            dynamic response = await Requests.POST(jdPOSTObject, uri);

            bool isSuccess;
            try
            {
                isSuccess = Convert.ToInt32(response.output) == userTaskCode.OutputParameter;
            }
            catch
            {
                isSuccess = false;
            }
            

            return isSuccess;
        }

    }
}
