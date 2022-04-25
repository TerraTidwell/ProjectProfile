//
//  Hoop.cpp
//  FinalProject
//
//  Created by Spencer Westover on 9/22/21.
//

#include "Hoop.hpp"


void Hoop :: drawHoop(sf::RenderWindow & window) {
    
    hoop_.setOrigin(100,100);
    hoop_.setPosition( x_, y_);
    sf:: Texture hp;
    hp.loadFromFile("Hoop.png");
    hoop_.setTexture(&hp);
    
    window.draw(hoop_);
}

void Hoop :: update(sf::RenderWindow & window, int & dirX, int & dirY){


    if (hoop_.getPosition().x <= 50) {
        dirX = 1;
      }
    else if ((hoop_.getPosition().x) + (hoop_.getRadius()*2) >= window.getSize().x+75){

        dirX = 0;
    }
    if (dirX == 1) {
//
        x_ += 5;
    }
    else if (dirX == 0) {
//
        x_ -= 5;
    }
    
    if (hoop_.getPosition().y <= 50) {
        dirY = 1;
      }
    else if ((hoop_.getPosition().y) + (hoop_.getRadius()*2) >= window.getSize().y+75){

        dirY = 0;
    }
    if (dirY == 0) {
//
        y_ -= 5;
    }
    else if (dirY == 1) {
//
        y_ += 5;
    }
}

