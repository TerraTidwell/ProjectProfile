//
//  Basketball.cpp
//  FinalProject
//
//  Created by Spencer Westover on 9/22/21.
//

#include "Basketball.hpp"
#include <math.h>


void Basketball:: initialPosition (sf::RenderWindow & window) {
    x_ = 25;
    y_ = 25;
    
    ball_.setPosition( x_, y_);
    sf::Texture ball;
    ball.loadFromFile("basketball.png");
    
    ball_.setTexture(&ball);
    window.draw(ball_);
}

void Basketball :: drawBasketball (sf:: RenderWindow & window) {
        
        ball_.setPosition( x_, y_);
        sf:: Texture ball;
        ball.loadFromFile("basketball.png");
        ball_.setTexture(&ball);
        
        window.draw(ball_);
    }
void Basketball::update( sf::RenderWindow & window, int xCoord, int yCoord, int & xDir, int & yDir, bool & xMax ) {
    
    if (ball_.getPosition().y <= 25) {
        yDir = 1;
      }
    else if ((ball_.getPosition().y) + (ball_.getRadius()*2) >= window.getSize().y+25){

        yDir = 0;
    }
    if (yDir == 0) {
//
        y_ -= yCoord / 100;
    }
    else if (yDir == 1) {
//
        y_ += yCoord / 100;
    }
    
//    if (ball_.getPosition().x <= 25) {
//        xDir = 1;
//      }
    if ((ball_.getPosition().x) + (ball_.getRadius()*2) >= window.getSize().x+150){
        
        xMax = true;
        //xDir = 0;
   }
//    if (xDir == 1) {

        x_ += xCoord / 100;
    //}
//    else if (xDir == 0) {
//
//        x_ -= xCoord / 100;
//    }
    
  ball_.setPosition( x_, y_);
}

bool Basketball::collided(const Hoop &hoop )
{
    {
      float xdist2 = pow( hoop.x_ - x_, 2 );
      float ydist2 = pow( hoop.y_ - y_, 2 );
      float totalDistance = sqrt( xdist2 + ydist2 );
       
      if( totalDistance < (size_ + hoop.size_)/3) {
        return true;
      }
      else {
        return false;
      }
       
    }
   

}
