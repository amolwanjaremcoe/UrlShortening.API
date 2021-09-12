The scope/goals of the solution
1. For any given valid URL, service should create/generate a shorter and unique alias. This link should be short. 
2. Service will check if URL is valid and location is reachable before creating keys and adding to database.
3. When users access a short link, our service should redirect them to the original link.
4. Links will expire after a standard default timespan.
5. Service should Cache keys to the reduce database querying.

How to Run the application - 
1. Run the "docker compose up" command to install the required services (Mongo db, Redis) in the docker container.
2. Set the "UrlShortening.API" project as the start up application and run the project in IIS Express.
3. In order to Create the Short URL, send the POST request to localhost:44315 endpoint

/*****************************************Create the Short URL Request sample**********************************/
POST /short HTTP/1.1
Host: localhost:44315
Content-Type: application/json
Cache-Control: no-cache
Postman-Token: 71c4832f-ccc5-2574-ac34-051291dd6629

{
	"originalUrl":"https://dotnet.microsoft.com/learn/dotnet/what-is-dotnet-framework"
}
/*****************************************Create the Short URL Request sample**********************************/

4. In order to test the redirection, send the GET request to localhost:44315 endpoint by passing the Shortcode in the URL

/*****************************************Redirection Request sample**********************************/
GET /1iDNDY HTTP/1.1
Host: localhost:44315
Content-Type: application/json
Cache-Control: no-cache
Postman-Token: 7df8cc8c-4f6a-2f2d-fe3f-c3d290668caa
/*****************************************Redirection Request sample**********************************/

Assumptions:
1. This application will be read heavy (i.e. more GET requests than the create/generate POST requests ) 
2. We may need to store millions/billions of records in the database.
3. For the same URL request, user will receive different Shortened URL everytime.

Implementation approach :

1. We will build a rest API (present in the UrlShortening.API project) to expose the functionality of our application.
2. As each data object we store is small and we don't need to store the relationsheeps between records we can select NOSQL database like Mongo.
3. We will use MD5 hashing algorith to Hash the given URL by adding the current date tick in the value to generate unique hash code. And then Encode
   the hash value using Base62([A-Z, a-z, 0-9]) encoding and select the first SIX characters from the encoded string.
   Using base62 encoding, a 6 letters long key would result in 62^6 = ~56.8 billion possible strings.
4. We are using the Redis cache to cache the Redirection requests. It will use LRU as cache eviction scheme. It will cache the recent redirection requests in memory.


Future scope :
1. We can assign a Api-key to user requesting to generate shorter URL. This can be used to throttle users based on their allocated quota.
2. We can support sending the custom user defined alias which can be used as key in the shorter URL.
3. We can support user sending the Expiration date as part of create/generate request.

