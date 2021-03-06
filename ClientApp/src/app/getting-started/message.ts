﻿export const MESSAGE = [
  '<h4>Welcome</h4>' +
  '<p>This website demonstrates a simple application of a Partially Observable Monte-Carlo Planning (POMCP) method applied to a simple situation: a series of cameras following a person in a partially occluded environment. This situation is simplified down to a two-dimensional grid and is modeled by a partially observable Markov decision process.</p>' +
  '<img src="demo.gif" alt="Demo gif">' +
  '<div><div class="cell camera node"></div> Camera <div class="cell target node"></div> Target <div class="cell wall node"></div> Wall</div></div>',

  "<h4>What is a Partially Observable Markov Decision Process ?</h4> " +
  "<p>A <a href=\"https://en.wikipedia.org/wiki/Markov_chain\" target=\"_blank\">Markov chain</a> is a sequence of event with discrete time steps where the probability of each event depends only on the probability of the previous event. Here, at each step, the target can move to one of the grid spaces next to them." +
  "<p>When a Markov chain is <a href=\"https://en.wikipedia.org/wiki/Hidden_Markov_model\" target=\"_blank\">partially observable</a>, the real state of the system is not known for sure by the agent, but rather observed. Here, the cameras can observe the target or not, and when they do not, the true position is unknown by the agent controlling the cameras.</p>" +
  "<p> A <a href=\"https://en.wikipedia.org/wiki/Markov_decision_process\" target=\"_blank\">Markov decision process</a>, on the other hand, means that an agent will decide on every time step to act on the system. Here, the agent will choose to rotate the cameras a certain way to try to observe the target. The camera can only rotate 45° at a time</p>" +
  "<p>A <a href=\"https://en.wikipedia.org/wiki/Partially_observable_Markov_decision_process\" target=\"_blank\">partially observable Markov decision process</a> is the combination of all these: a system that evolves on a discrete timeline where an agent has to make a decision each time step without knowing the real state of the system.</p>" +
  '<img src="evolution-example.gif" alt="POMDP Evolution">' +
  '<div><div class="cell camera node"></div> Camera <div class="cell target node"></div> Target <div class="cell wall node"></div> Wall</div></div>',



  '<h4>Partially Observable Monte-Carlo Planning</h4>' +
  '<p>The objective of this application is to automate the decision process. In other words, to be able to automatically orient the cameras so that they follow the target. The solution presented here is an implementation of the POMCP method. This method makes a decision by building a tree of the possible future actions and future observations over several time steps and then pick the immediate best action by evaluating its value not only based on the immediate reward but also the potential future implications of the decision. For example, rotating a camera 45° might lead to only observing a wall but rotating it again in the next step could allow for observing a different part of the map where the target could be.</p>' +
  '<img src="tree-growing.gif" alt="Tree Search">' +
  '<div><div class="node starting-node"></div> Current Observation <div class="node action-node"></div> Action <div class="node obs-node"></div> Observation </div>',


  '<h4>Setting up your environment</h4>' +
  '<p>The environment is fully customizable! You can choose one of the available maps in the maps menu, or if you want or create your own you can modify the size of the map, its content, the number of cameras and everything on it! Note that modifying the environment of the experiment will reset the system as it is only made to handle the target’s movement.</p>' +
  '<img src="map-editing.gif" alt="Map editing example">',


  '<h4>Exploring the world</h4>' +
  '<p>You are in control of the character! Try moving around the map using the directional arrows on the screen and see if the cameras can follow you even when after you are hidden. In a normal POMDP situation, the system would evolve on its own using defined rules, the manual control just adds some interactivity. To simulate the evolution of process, the camera agent assumes the target moves at random: this is not a flawless approximation but should be enough to follow you around in most cases</p>' +
  '<img src="movement.jpg" alt="Movement arrows">' +
  '<div> <i class="fa fa-arrow-circle-o-right"></i> Move in the direction of the arrow <i class="fa fa-circle-o"></i> Stay put (one time step will pass)</div>',


  '<h4>More Information</h4>' +
  '<p>The POMCP method is a rather complex system with a few subtilities that can not be explained in only a few lines, the interested reader can refer to the following materials to better understand all the details of this method</p>' +
  '<ul>' +
  '<li><a href=\"https://en.wikipedia.org/wiki/Monte_Carlo_tree_search\" target=\"_blank\">Monte Carlo Tree Search</a> (Wikipedia)</li>' +
  '<li><a href=\"https://papers.nips.cc/paper/4031-monte-carlo-planning-in-large-pomdps.pdf\" target=\"_blank\">Monte-Carlo Planning in Large POMDPs</a> (Research article) </li>' +
  '<li><a href=\"http://proceedings.mlr.press/v70/katt17a/katt17a.pdf\" target=\"_blank\">Learning in POMDPs with Monte Carlo Tree Search</a> (Research article) </li>' +
  '</ul>',
  ];
