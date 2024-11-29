Hi!

There are 2 options to start the app:

via DOCKER
	To run the app you need a Docker engine (Docker Desktop will do the trick)

	1. Open the root folder
	2. Run 'docker-compose up --build'
	3.  The Api will be accessible from http://localhost:5028/swagger/index.html
		The Client - http://localhost:7172/

Locally via Visual Studio 2022
	
	1. Open solution in VS
	2. Solution properties -> Run Multiple -> Select 'START' at BookClient and BookServer
	3. Run the app
	4.  Api will be accessible at http://localhost:5028/swagger/index.html
	     Client - http://localhost:7172/

FYI. The UploadExample.csv in the BookManagement\BookServer folder