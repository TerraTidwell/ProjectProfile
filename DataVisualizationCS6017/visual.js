


 var margin = {top: 20, right: 20, bottom: 30, left: 75},
 width = 960 - margin.left - margin.right,
 height = 500 - margin.top - margin.bottom; 

var x = d3.scaleBand()
 .range([0, width])
 .padding(0.1);
var y = d3.scaleLinear()
 .range([height, 0]); 



window.onload = async function() {

function createGraph() {
d3.csv("COVID.csv").then(function(data) {
    data.forEach(function(d){
      d["Deaths - cumulative total"] = +d["Deaths - cumulative total"]; //set to an integer
    })

x.domain(data.map(function(d) {return d.Name; })); //set range for x
y.domain([0, d3.max(data, function(d) { return +d["Deaths - cumulative total"]; })]); //set range for y


var svg = d3.select("div").append("svg") //set the staging
     .attr("width", width + margin.left + margin.right)
     .attr("height", height + margin.top + margin.bottom)
     
     .append("g")
     .attr("transform", 
          "translate(" + margin.left + "," + margin.top + ")")



svg.selectAll(".bar") //qualities for bar graph
.data(data)
.enter().append("rect")
.attr("class", "bar") 
.on("mouseover", onMouseOver) //function to highlight bars
.on("mouseout", onMouseOut) //handle mouse off
//.on("click", onMouseClick)
.attr("x", function(d) { return x(d.Name); })
.attr("width", x.bandwidth())
.attr("y", function(d) {  return y(+d["Deaths - cumulative total"]); })
.attr("height", function(d) { return height - y(+d["Deaths - cumulative total"]); }); 


svg.append("g") //x -axis
 .attr("transform", "translate(0," + height + ")")
 .call(d3.axisBottom(x));
 
svg.append("text") //graph label
 .attr("class", "x label")
 .attr("text-anchor", "end")
 .attr("x", width)
 .attr("y", height - 40)
 .text("March 2020 - June 2022");
// Add y axis
svg.append("g")
 .call(d3.axisLeft(y));
 
 
});
}
createGraph(); //function call to display graph
}

function onMouseOver(d, i) {
  //var data = d3.csv("COVID.csv");
  d3.select(this).attr('class', 'highlight');
  d3.select(this)
    .transition()     // adds animation
    .duration(400)
    //.call(d3.axisLeft(y))
    .attr('width', x.bandwidth())
    .attr("y", function(d) { return y(+d["Deaths - cumulative total"]); })
    .attr("height", function(d) { return height - y(+d["Deaths - cumulative total"]); });

  div2.append("text")
   //.data(d)
   .attr('class', 'val') 
   .attr('x', function() {
       return x(d.Name);
   })
   .attr('y', function() {
       return y(+d["Deaths - cumulative total"]);
   })
   .call(d3.axisLeft(y))  
   .text(function() { 

      return ["COUNTRY: " + d.Name + " ........................ " + "Total Deaths: " + d["Deaths - cumulative total"]
                + " ...................... " + "Total Cases: " + d["Cases - cumulative total"]
              + " ................. "+ "Deaths per capita: " + d["Deaths - cumulative total per 1000000 population"]
            + " .............. " + "Cases per capita: " + d["Cases - cumulative total per 1000000 population"] + "............"];
  
      // Value of the text
   });
}
var div2 = d3.select("body").append("div") //div box to the side
  .attr("class", "box")
  
// var div3 = d3.select("body").append("div") //div box to the side
//   .attr("class", "smallbox")
//   .attr("height", height + 20);
//add mouseover event 

function onMouseOut(d, i) {
  // use the text label class to remove label on mouseout
  d3.select(this).attr('class', 'bar');
  d3.select(this)
    .transition()     // adds animation
    .duration(400)
    .attr('width', x.bandwidth())
    .attr("y", function(d) { return y(+d["Deaths - cumulative total"]); })
    .attr("height", function(d) { return height - y(+d["Deaths - cumulative total"]); });

  d3.selectAll('.val')
    .remove()
  } 
  
// function onMouseClick(d, i) {
//   d3.select(this).attr('class', 'highlight');
//   d3.select(this)
//     .transition()     // adds animation
//     .duration(400)
//     .call(d3.axisLeft(y))
//     .attr('width', x.bandwidth())
//     .attr("y", function(d) { return y(+d["Deaths - cumulative total"]); })
//     .attr("height", function(d) { return height - y(+d["Deaths - cumulative total"]); });

//     div3.append("text")
//     .attr('class', 'val') 
//     .attr('x', function() {
//         return x(d.Name);
//     })
//     .attr('y', function() {
//         return y(+d["Deaths - cumulative total"]);
//     })
//     .attr("text-anchor", "end")
//     .text(function() {
 
//        return [d["Deaths - cumulative total"]];
   
//        // Value of the text
//     });
//}















 
 