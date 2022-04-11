1.	I developed the coding assignment in about 4 days. If I had more free time, I will develop these tasks for it:

      a.	an appropriate UI 

     b.	use a real database and write integration tests

     c.	Exception handling with middleware 

    d.	more feature for user: Delete a code from the list, clear the output list

     e.	add logging 

2.	 One of the features C# 8.0 (version I’m using) added to C# is Static local functions. You can add a static modifier to local function doesn’t capture (reference) any variables from the enclosing scope. I used this in CryptoService class:
    Inline-style: 
![alt text](https://github.com/nilooferAbdolalizade/KnabCryptoCurrency/localStaticFunction.png "localStaticFunction")

3.	At first, I will try to find out in what part of the code the problem has occurred. Depend on the business and code, I use some tools like Profiler, diagnostic or Benchmark to find which part has weakened the performance. 
I remember Last Year I published a new windows service which had to get huge data from a database and do some business on them and write the new data on another database. Writing the set of records that should normally take few milliseconds took one minute and caused deadlock on database. It took a long time to find out that part of the code and try to improve the query and fix the performance problem.

4.	In recent months, for making decisions to dockerize our project or not, I searched about it and read some essays. I also attended an online conference to get more familiar with it. And I realized we have to dockerize our project as soon as possible to develop and publish faster and easier. Because, All the pros of docker are useful for our project.

5.	I think it was a very good assessment for understanding the developer’s code style and way of thinking. Writing clean code and unit tests are very important part of coding and even the structure has been chosen by developer. It was not easy to write in a few hours, Which does not give much information to the code reviewer and it was not so difficult for the developer to focus only on fixing the problem at the specified time and not have enough time to clear their code or add some value.

6.	Me :

{ 
"Niloofar": {
"NickName" : ["Niloo", "Niloofer"],
"Features": [
"curly hair",
"proactive",
"camper",
"social",
"energetic",
"persistence",
"planner",
"accepting new challenges",
"nature lover",
"self-learning",
"fast learner",
"team player",
"leadership",
"good organizer",
"resilience",
"punctual",
"integrity",
"reliable"
],
	"DreamJob" : "becoming CTO in future",
	"hobby" : [
		"hiking  and trekking",
		"camping",
		"playing some musical instrument: Sitar, Tar, Harmonica and Kalimba",
		"hanging out with friends",
		"traveling",
		"listening to the podcast",
		"reading books"
		]
}
}

