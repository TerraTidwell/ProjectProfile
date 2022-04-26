//
//  main.cpp
//  msdscript
//
//  Created by Terra Tidwell on 1/11/22.
//

#include <iostream>
#include "cmdline.hpp"

int main(int argc, const char * argv[]) {
    try{
          use_arguments(argc, argv);
          return 0;
      } catch (std::runtime_error exn){
          std::cerr << exn.what() << "\n";
          return 1;
      }
}

