//
//  step.hpp
//  msdscript
//
//  Created by Terra Tidwell on 3/29/22.
//

#ifndef step_hpp
#define step_hpp

#include <stdio.h>
#include "Expr.hpp"
#include "env.hpp"
#include "val.hpp"
#include "Cont.hpp"



class Expr;
class Env;
class Val;
class Cont;


CLASS (Step){
public:
    typedef enum{
        interp_mode,
        continue_mode
    } mode_t;

static mode_t mode;
static PTR(Expr) expr;
static PTR(Env) env;
static PTR(Val) val;
static PTR(Cont) cont;
    
static PTR(Val) interp_by_steps(PTR(Expr) e);
};


#endif /* step_hpp */
