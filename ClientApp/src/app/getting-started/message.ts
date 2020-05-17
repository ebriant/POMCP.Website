export const MESSAGE = [
  '<h4>Welcome</h4>' +
  '<p>This application demontrate a simple application of a Partially Observable Monte-Carlo Planning (POMCP) system applied to a simple situation: a camera following a person in an occluded environment</p>' +
  '<img src="small_room.jpg" alt="Markov model">',

  "<h4>What is a Partially Observable Markov Decision Process ?</h4> " +
  "<p>A <a href=\"https://en.wikipedia.org/wiki/Markov_chain\" target=\"_blank\">Markov chain</a> is a sequence of event with discrete time steps where the probability of each event depends only on the probability of the previous event. Here, the markov model is partially observable (or <a href=\"https://en.wikipedia.org/wiki/Hidden_Markov_model\" target=\"_blank\">hidden</a>), meaning the state of the system is not known directly to the agent, but rather observed with the true state of the system not known for sure. Here, the cameras can observe the target or not, and when they do not, the true position is unknown. This is also a <a href=\"https://en.wikipedia.org/wiki/Markov_decision_process\" target=\"_blank\">Markov decision process</a>, meaning an agent will take a decision on every time step to act on the system: here, the agent will choose to rotate the cameras a certain way to try to observe the target.</p>" +
  '<img src="Basic-Movement.gif" alt="Markov model">',

  '<h4>Partially Observable Monte-Carlo Planning</h4>' +
  '<p>The objective of this application is to be able to automatically take a decision regarding the system. The solution presented here is an implementation of the Partially Observable Monte-Carlo Planning (POMCP) method. This method is allowed to make a decision by buiding a tree of the possible future actions and future observations over several time steps and then pick the immediate best action by evaluating its value not only based on the immediate reward but also the potential futur implications of the decision.</p>' +
  '<img src="" alt="Markov model">',


  '<h4>Setting up your environment</h4>' +
  '<p>The environment is fully customzable ! You can modify the size of the map, its content, the number of cameras and so on. Note that modifying the environment of the experiment resets the system as it is not made to handle it (a more complex pomcp could, but that is not the goal of this visualization)</p>' +
  '<img src="" alt="Markov model">',
  ];
