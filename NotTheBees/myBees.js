

"use strict";

let canvas = document.getElementsByTagName('canvas')[0];
canvas.style = "border: 1px solid black; padding: 20px;";
let ctx = canvas.getContext('2d');
let bees = [];
let done = false;
let startTime = new Date();
//load background image
let backgroundImage = new Image();
backgroundImage.src = "Background.png";
//////////////////////////
//load flower image
let flowerImage = new Image();
flowerImage.src = "Flower_Image.png";

///////////////////////////////
////create the bees
let numBees = 10;

for(let i = 0; i < numBees; i++){

    let bee = {img: {}, speed: Math.random()/2.0 + 0.5,xPos:Math.floor(Math.random() * 950), yPos:Math.floor(Math.random() * 450)};

    bee.img = new Image();

    bee.img.src = "BeePic.png";

    bees.push(bee);
}


//Did the bee touch the flower?
function beeOnFlower(bee){

    let tarX = flowerImage.xPos;
    let tarY = flowerImage.yPos;

    let distance = Math.sqrt(Math.pow(bee.xPos - tarX,2) + Math.pow(bee.yPos - tarY, 2));
    if (distance < 20){
        done = true;

    }
}

function updateScreen(){
    ctx.clearRect(0,0,canvas.width, canvas.height);
    ctx.drawImage(backgroundImage, 0, 0, canvas.width, canvas.height);

    let widthOffset = flowerImage.width/2;
    let heightOffset = flowerImage.height / 2;

    ctx.drawImage(flowerImage, flowerImage.xPos, flowerImage.yPos, 100, 100);

    ////////////////////////////
    //Update the bees
    let tarX = flowerImage.xPos;
    let tarY = flowerImage.yPos;

    for (let i = 0; i < bees.length; i++) {
        let bee = bees[i];

        if (bee.xPos > tarX) {
            bee.xPos -= bee.speed;
            bee.img.reverse = true;
            if (bee.xPos < tarX) {
                bee.xPos = tarX;
            }
        }
        else if (bee.xPos < tarX) {
            bee.xPos += bee.speed;
            bee.img.reverse = false;
            if (bee.xPos > tarX) {
                bee.xPos = tarX;
            }
        }

        if (bee.yPos > tarY) {
            bee.yPos -= bee.speed;
            if (bee.yPos < tarY) {
                bee.yPos = tarY;
            }
        } else if (bee.yPos < tarY) {
            bee.yPos += bee.speed;
            if (bee.yPos > tarY) {
                bee.yPos = tarY;
            }
        }
        beeOnFlower(bee);
    }

    ///////////////////////////
    //Draw the bees
    for (let i = 0; i < bees.length; i++){
        let bee = bees[i];

        let xPos = bee.xPos;
        let yPos = bee.yPos;

        ctx.drawImage(bee.img, xPos, yPos,35,35);
    }
    ////////////////////////////////
    ///update the bees speed

    let endTime = new Date();
    let timeDiff = (endTime - startTime) / 1000; //Convert to seconds

    if (timeDiff > 5){
        startTime = endTime;
        for (let i = 0; i < bees.length; i++){
            let bee = bees[i];
            bee.speed *= 2;
        }
    }

    ///////////////////////////
    //Request the next frame.......
    if(done === false){
        window.requestAnimationFrame(updateScreen);

    }
}

function handleMouseMovement(e){
    ctx.clearRect(0,0,canvas.width, canvas.height);
    ctx.drawImage(backgroundImage, 0, 0, canvas.width, canvas.height);
    flowerImage.xPos = e.x;
    flowerImage.yPos = e.y;
    let widthOffset = flowerImage.width/2;
    let heightOffset = flowerImage.height / 2;

    ctx.drawImage(flowerImage, flowerImage.xPos - canvas.offsetLeft-widthOffset, flowerImage.yPos-canvas.offsetTop-heightOffset, 100, 100);

}



document.onmousemove = handleMouseMovement;
updateScreen();


