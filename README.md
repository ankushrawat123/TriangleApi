# TriangleApi

REQUEST RESPONSE


Resquest
{
  "firstSide": 10,
  "secondSide": 10,
  "thirdSide": 10
}

	
Response body
{
  "data": "EQUILATERAL",
  "code": 200,
  "status": "Success",
  "isSuccess": true,
  "message": "Request Processed Successfully",
  "developerMessage": null
}

---------------------------------------------------

Resquest
{
  "firstSide": 10,
  "secondSide": 10,
  "thirdSide": 20
}	
	
Response body
{
  "data": "ISOSCELES",
  "code": 200,
  "status": "Success",
  "isSuccess": true,
  "message": "Request Processed Successfully",
  "developerMessage": null
}
----------------------------------------------------

Resquest	
{
  "firstSide": 10,
  "secondSide": 30,
  "thirdSide": 20
}	
	
Response body
{
  "data": "SCALANE",
  "code": 200,
  "status": "Success",
  "isSuccess": true,
  "message": "Request Processed Successfully",
  "developerMessage": null
}



-----------------------------------------------------
Resquest
{
  "firstSide": -10,
  "secondSide": 30,
  "thirdSide": 80
}


Response body

{
  "data": null,
  "code": 301,
  "status": "InvalidFirstSideInput",
  "isSuccess": false,
  "message": "FirstSide Input Value Is Either Zero Or Less Than Zero",
  "developerMessage": null
}


----------------------------------------------
Resquest
{
  "firstSide": 10,
  "secondSide": -30,
  "thirdSide": 80
}
	
Response body

{
  "data": null,
  "code": 302,
  "status": "InvalidSecondSideInput",
  "isSuccess": false,
  "message": "SecondSide Input Value Is Either Zero Or Less Than Zero",
  "developerMessage": null
}


------------------------------------------
Resquest

{
  "firstSide": 10,
  "secondSide": 30,
  "thirdSide": -80
}
	
Response body

{
  "data": null,
  "code": 303,
  "status": "InvalidThirdSideInput",
  "isSuccess": false,
  "message": "ThirdSide Input Value Is Either Zero Or Less Than Zero",
  "developerMessage": null
}
--------------------------------------------



