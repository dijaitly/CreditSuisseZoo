i) Open the folder CreditSuisse.QIS.VendingMachine and then open CreditSuisse.QIS.VendingMachine.sln file in Visual Studio  2017 
ii) Open the solution explorer make the CreditSuisse.QIS.VendingMachine as startup project if it is not already as a start up project
iii) Launch the solution  in IIS express
iv) In fiddler compose a request 
as follows

POST http://localhost:50931/api/VendingMachine/Vend HTTP/1.1
User-Agent: Fiddler
Content-Type: application/json
Host: localhost:50931
Content-Length: 45

{CardNumber:1234567,PIN:1235,NumberOfCans:14}


v) CardNumber and pins can be found in the Load method of Repository class found in CreditSuisse.QIS.Repository
vi) There are two tables Cards and Accounts with relationship of Accounts being parent and Cards being children table and containing reference of acccount number
vii) CardNumber in Cards should be unique and AccountNumber in Account should be unique. 
viii) This can be hosted in IIS as well if needed
ix) UnitTests are in the project CreditSuisse.QIS.VendingMachine.Tests
x) Tranasctions are not maintained in the solution
