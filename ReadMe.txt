##The scope/goals of the solution
1. For any given valid URL, service should create/generate a shorter and unique alias link. This link should be short. 
2. Service will check if given original URL is valid and is reachable before creating keys and adding to database.
3. When users access a short link, our service should redirect them to the original link.
4. Links will expire after a standard default timespan (Five years).
5. Service should Cache keys to the reduce database querying and improve the Redirection time.

#Assumptions:
1. This application will be read heavy (i.e. more GET requests than the create/generate POST requests ) 
2. We may need to store millions/billions of records in the database.
3. For the same URL request, user will receive different Shortened URL everytime.

#Implementation approach :

1. We will build a rest API (present in the UrlShortening.API project) using .Net core API to expose the functionality of our application.
2. We will expose two endpoints for users to consume - 
	a. GET endpoint which accepts the {shortCode} and will perform the redirection to actual URL
	b. CreateShortUrl POST endpoint which accepts the {OriginalUrl} and will generate Unique short code, validate the shortcode is 
	   not used before, return the Shortened URL to end user
3. As each data object we store is small and we don't need to store the relationsheeps between records we can select NOSQL database like Mongo.
4. We will use MD5 hashing algorith to Hash the given URL by adding the current date tick in the value to generate unique hash code. And then Encode
   the hash value using Base62([A-Z, a-z, 0-9]) encoding and select the first SIX characters from the encoded string.
   Using base62 encoding, a 6 letters long key would result in 62^6 = ~56.8 billion possible strings.
5. We are using the Redis cache to store the Redirection requests. It will cache the recent redirection requests in memory.
   Data will be returned from the Cache if found in the cache. We will set the cache expiration to 4 hours. 
   This value can be adjusted as per the request pattern.


#How to Run the application - 
1. Run the "docker compose up" command to install the required services (Mongo db, Redis) in the docker container.
2. Set the "UrlShortening.API" project as the start up application and run the project in IIS Express.
3. In order to Create the Short URL, send the POST request to localhost:44315 endpoint

/*****************************************Create the Short URL CURL Request sample**********************************/
POST /short HTTP/1.1
Host: localhost:44315
Content-Type: application/json
Cache-Control: no-cache

{
	"originalUrl":"https://dotnet.microsoft.com/learn/dotnet/what-is-dotnet-framework"
}
/*****************************************Create the Short URL Request sample**********************************/

4. In order to test the redirection, send the GET request to localhost:44315 endpoint by passing the Shortcode in the URL

/*****************************************Redirection CURL Request sample**********************************/
GET /1iDNDY HTTP/1.1
Host: localhost:44315
Content-Type: application/json
Cache-Control: no-cache
/*****************************************Redirection Request sample**********************************/

#Future scope :
1. We can assign a Api-key to user requesting to generate shorter URL. This can be used to throttle users based on their allocated quota.
2. We can support sending the custom user defined alias which can be used as key in the shorter URL.
3. We can support user sending the Expiration date as part of create/generate request.
4. We can create seperate Key generation component which will keep generating code in its Key database and API service would use
this component provided Code value.
5. Add analytics and capture various details such as Redirection requests count, Requests to create short URL etc.

