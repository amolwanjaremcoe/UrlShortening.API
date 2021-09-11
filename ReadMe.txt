The scope/goals of the solution
1. For any given valid URL, service should create/generate a shorter and unique alias. This link should be short. 
2. Service will check if URL is valid and location is reachable before creating keys and adding to database.
3. When users access a short link, our service should redirect them to the original link.
4. Links will expire after a standard default timespan.
5. Service should Cache keys to the reduce database querying.


Assumptions:
1. This application will be read heavy (i.e. more GET requests than the create/generate POST requests ) 
2. We may need to store millions/billions of records in the database.

Implementation approach :

1. We will build a rest API (present in the UrlShortening.API project) to expose the functionality of our application.
2. As each data object we store is small and we don't need to store the relationsheeps between records we can select NOSQL database like Mongo.
3. 



Future scope :
1. We can assign a Api-key to user requesting to generate shorter URL. This can be used to throttle users based on their allocated quota.
2. We can support sending the custom user defined alias which can be used as key in the shorter URL.
3. We can support user sending the Expiration date as part of create/generate request.

