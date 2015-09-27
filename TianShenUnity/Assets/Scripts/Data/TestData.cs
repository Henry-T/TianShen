using UnityEngine;
using System.Collections;
using AVOSCloud;

public class TestData
{
	string testA=string.Empty;
	string objetcId=string.Empty;

	public static void DB_CreateTestData(string input)
	{
		AVObject obj = new AVObject("Test");
		obj["TestA"] = input;
		
		obj.SaveAsync().ContinueWith(t => {
			TestData testObject = new TestData();
			testObject.testA = input;
		});
	}

	public static void DB_UpdateTestData(string changedData)
	{
		/**
		string objectUpdateId = string.Empty;
		AVQuery<AVObject> query=new AVQuery<AVObject>("Test");
		query.WhereEqualTo("TestA","testa").FirstAsync().ContinueWith(t =>{
			objectUpdateId=t["TestA"];
		}
		);**/

		var obj = new AVObject ("Test");
		
		obj.ObjectId = "53b7bc82e4b0cdeb6e249b7a";
		obj["TestA"] = changedData;
		obj.SaveAsync().ContinueWith(t =>
		                                   {
			// 保存成功之后，修改一个已经在服务端生效的数据，这里我们修改cheatMode和score
			// AVOSCloud只会针对指定的属性进行覆盖操作，本例中的playerName不会被修改
			//obj["TestA"] = "changedTesta";
			//obj.SaveAsync();
		});
	}
}
