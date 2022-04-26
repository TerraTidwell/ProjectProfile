//
//  cmdline.cpp
//  msdscript
//
//  Created by Terra Tidwell on 1/11/22.
//

#define CATCH_CONFIG_RUNNER
#include "catch.h"
#include "cmdline.hpp"
#include <functional>
#include <string>
#include <iostream>




void use_arguments(int argc, const char * argv[]){
    //if no argument is provided
    if(argc == 1) {
        exit(0);
    }
    bool testUsed = false;
    for(int i = 1; i < argc; i ++) {
        std::string arg = argv[i];
        if(arg == "--help") {
            std::cout << "You are allowed to use the following arguments: '--help'\n, '--test'\n, '--interp'\n, '--step', '--print'\n, '--pretty-print'\n";
            exit(0);
        } else if (arg == "--test") {
            if(testUsed == false){
                if (Catch::Session().run(1,argv) != 0){
                    exit(1);
                }
                std::cout << "Tests passed\n";
            
            testUsed = true;
                
            }
            else {
            std::cerr << "Argument already used\n";
            exit(1);
        }
    
    
} else if (arg == "--interp"){
           PTR(Expr)  user_input = parse_expr(std::cin);
    std::cout << user_input->interp(Env::empty)->to_string();
    std::cout << "\n";
    exit(0);
        
       } else if (arg == "--print"){
           PTR(Expr) user_input = parse_expr(std::cin);
           user_input->print(std::cout);
           exit(0);
       } else if (arg == "--pretty-print"){
           PTR(Expr)  user_input = parse_expr(std::cin);
           std::cout << user_input->to_string_pretty();
           exit(0);
       } else if (arg == "--step"){
           PTR(Expr) user_input = parse_expr(std::cin);
           PTR(Val) user_val = Step::interp_by_steps(user_input);
           std::cout << user_val->to_string();
           std::cout << "\n";
           exit(0);
        
    
           
           
       }else {
                   std::cerr << "Invalid argument\n";
                   exit(1);
               }
               std::cout << "\n";
    }
    
   
}
