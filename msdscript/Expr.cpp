//
//  Expr.cpp
//  msdscript
//
//  Created by Terra Tidwell on 1/16/22.
//

#include "Expr.hpp"
#include "val.hpp"
#include "catch.h"
#include "parse.hpp"
#include <stdexcept>
#include <string>
using namespace std;
#include <sstream>


std::string Expr:: to_string() {
    std::stringstream output("");
    this->print(output);
    return output.str();
}

std::string Expr:: to_string_pretty(){
    std::stringstream output("");
    this->pretty_print(output);
    return output.str();
}

void Expr::pretty_print(std::ostream& output) {
    pretty_print_at(output, print_none, 0, 0);
}

/*////////////////////CONSTRUCTORS//////////////////////////////////////////////////////////////////////////////////*/


NumExpr :: NumExpr(int val) {
    this-> rep = val;
}


AddExpr :: AddExpr (PTR(Expr)  lhs, PTR(Expr)  rhs) {
    this -> lhs = lhs;
    this -> rhs = rhs;
    
}


MultExpr :: MultExpr (PTR(Expr)  lhs, PTR(Expr)  rhs) {
    this -> lhs = lhs;
    this -> rhs = rhs;
    
}

VarExpr :: VarExpr (std:: string name) {
    this -> name = name;
    
}

LetExpr :: LetExpr(std::string lhs, PTR(Expr) rhs, PTR(Expr) body) {
    this-> lhs = lhs;
    this -> rhs = rhs;
    this -> body = body;
}

BoolExpr :: BoolExpr(bool val) {
    this->val = val;
}


IfExpr:: IfExpr(PTR(Expr) test, PTR(Expr)then, PTR(Expr) else_) {
    this->test = test;
    this->then = then;
    this->else_ = else_;
    
}

EqExpr:: EqExpr(PTR(Expr) lhs, PTR(Expr) rhs) {
    this->lhs = lhs;
    this->rhs = rhs;
}

FunExpr :: FunExpr(std::string formal_arg, PTR(Expr) body) {
    this->formal_arg = formal_arg;
    this->body = body;
    
}

CallExpr::CallExpr(PTR(Expr)to_be_called, PTR(Expr)actual_arg){
    this->to_be_called = to_be_called;
    this->actual_arg = actual_arg;
}

/*///////////////////////////EQUALS METHOD/////////////////////////////////////////////////////////////////////////*/

bool NumExpr :: equals ( PTR(Expr)  e) {
    
    PTR(NumExpr) n = CAST(NumExpr)(e);
    if (n == NULL)
          return false;
      else
          return this -> rep == n->rep;
}

bool AddExpr ::equals ( PTR(Expr)  e) {
    PTR(AddExpr) a = CAST(AddExpr)(e);
    if (a == NULL)
           return false;
    else
return ((this->lhs)->equals(a->lhs) && (this->rhs)->equals(a->rhs));
    
}

bool MultExpr :: equals ( PTR(Expr)  e) {
    PTR(MultExpr) m = CAST(MultExpr)(e);
    if(m == NULL) {
        return false;
    }
    else
        return (lhs->equals(m-> lhs)) && (rhs->equals(m -> rhs));
}


bool VarExpr :: equals ( PTR(Expr)  e) {
    PTR(VarExpr) v = CAST(VarExpr)(e);
    if ( v == NULL) {
        return false;
    }
    else
        return
        this->name == v -> name;
}

bool LetExpr :: equals( PTR(Expr)  e) {
    PTR(LetExpr) a = CAST(LetExpr)(e);
    if (a == NULL)
           return false;
    else
return ((this->lhs)==(a->lhs)) && ((this->rhs)->equals(a->rhs)) && ((this->body)->equals(a->body));
}


bool BoolExpr::equals( PTR(Expr)  e) {
    PTR(BoolExpr) a = CAST(BoolExpr)(e);
    if(a == NULL) {
        return false;
    } else{
        return (this -> val == a->val);
    }
    
}


bool IfExpr::equals( PTR(Expr)  e) {
    PTR(IfExpr) a = CAST(IfExpr)(e);
    if(a == NULL) {
        return false;
    }else {
        return (this->test)->equals(a->test) && (this->then)->equals(a->then) && (this->else_)->equals(a->else_);
    }
}

bool EqExpr::equals( PTR(Expr)  e) {
    PTR(EqExpr) a = CAST(EqExpr)(e);
    if (a == NULL) {
        return false;
    }
    else {
        return (this->lhs)->equals(a->lhs) && (this->rhs)->equals(a->rhs);
    }
}

bool FunExpr::equals( PTR(Expr)  e) {
    PTR(FunExpr) a = CAST(FunExpr)(e);
    
    if(a == NULL) {
        return false;
    }
    else {
        return(this->formal_arg)==(a->formal_arg) && (this->body)->equals(a->body);
    }
}


bool CallExpr::equals(PTR(Expr)other){
    PTR(CallExpr)other_num = CAST(CallExpr)(other);
    if (other_num == NULL){
        return false;
    } else {
        return (this->to_be_called)->equals(other_num->to_be_called) && (this->actual_arg)->equals(other_num->actual_arg);
    }
}

/*////////////////////////INTERP METHOD////////////////////////////////////////////////////////////////////////////*/


PTR(Val) NumExpr :: interp(PTR(Env) env) {
    return NEW (NumVal)(this-> rep);
}

PTR(Val) AddExpr :: interp(PTR(Env) env) {
    PTR(Val) lhs_val = lhs->interp(env);
    PTR(Val) rhs_val = rhs->interp(env);
    return lhs_val->add_to(rhs_val);
}


PTR(Val) MultExpr :: interp(PTR(Env) env) {
    PTR(Val) lhs_val = lhs->interp(env);
    PTR(Val) rhs_val = rhs->interp(env);
    return lhs_val->mult_to(rhs_val);
}

PTR(Val) VarExpr :: interp(PTR(Env) env) {
    return env->lookup(this->name);
    
}

PTR(Val) LetExpr :: interp(PTR(Env) env){
    PTR(Val) rhs_val = this->rhs->interp(env);
    
    PTR(Env) new_env = NEW(ExtendedEnv)(lhs, rhs_val, env);
    
    return this->body->interp(new_env);
}



PTR(Val) BoolExpr::interp(PTR(Env) env) {
    return NEW (BoolVal)(this->val);
}


PTR(Val) IfExpr :: interp(PTR(Env) env) {
    if((test->interp(env))->is_true()) {
        return then->interp(env);
    }
    else{
        return else_->interp(env);
    }
}

PTR(Val) EqExpr:: interp(PTR(Env) env){
    return NEW (BoolVal)((this->lhs->interp(env)->equals(this->rhs->interp(env))));
}

PTR(Val)FunExpr :: interp(PTR(Env) env) {
    return NEW (FunVal)(this->formal_arg, this->body, env);
}

PTR(Val) CallExpr::interp(PTR(Env) env){
    PTR(Val) to_be_called_val = to_be_called->interp(env);
    PTR(Val) actual_arg_val = actual_arg->interp(env);
    return to_be_called_val->call(actual_arg_val);
}

/*////////////////////////////STEP-INTERP////////////////////////////////////////////////////////*/

void NumExpr::step_interp() {
    Step::mode = Step::continue_mode;
    Step::val = NEW(NumVal)(rep);
    Step::cont = Step::cont; /* no-op */
}

void VarExpr::step_interp() {
     Step::mode = Step::continue_mode;
     Step::val = Step::env->lookup(name);
     Step::cont = Step::cont; /* no-op */
 }

void AddExpr::step_interp() {
     Step::mode = Step::interp_mode;
     Step::expr = lhs;
     Step::env = Step::env;
     Step::cont = NEW(RightThenAddCont)(rhs, Step::env, Step::cont);
 }

void MultExpr::step_interp() {
     Step::mode = Step::interp_mode;
     Step::expr = lhs;
     Step::env = Step::env;
     Step::cont = NEW(RightThenMultCont)(rhs, Step::env, Step::cont);
 }

void LetExpr::step_interp() {
     Step::mode = Step::interp_mode;
     Step::expr = rhs;
     Step::env = Step::env;
     Step::cont = NEW(LetBodyCont)(lhs, body, Step::env, Step::cont);
 }

void BoolExpr::step_interp() {
     Step::mode = Step::continue_mode;
     Step::val = NEW(BoolVal)(val);
     Step::cont = Step::cont; /* no-op */
 }


void EqExpr::step_interp() {
     Step::mode = Step::interp_mode;
     Step::expr = lhs;
     Step::env = Step::env; /* no-op, socouldomit */
     Step::cont = NEW(RightThenEqCont)(rhs, Step::env, Step::cont);
 }

void IfExpr::step_interp() {
     Step::mode = Step::interp_mode;
     Step::expr = test;
     Step::env = Step::env;
     Step::cont = NEW(IfBranchCont)(then, else_, Step::env, Step::cont);
 }

void FunExpr::step_interp() {
     Step::mode = Step::continue_mode;
     Step::val = NEW(FunVal)(formal_arg, body, Step::env);
     Step::cont = Step::cont; /* no-op */
 }

void CallExpr::step_interp() {
     Step::mode = Step::interp_mode;
     Step::expr = to_be_called;
     Step::cont = NEW(ArgThenCallCont)(actual_arg, Step::env, Step::cont);
 }





/*//////////////////////SUBST METHOD  --VOID--////////////////////////////////////////////////////////////////////////*/

//
//
//PTR(Expr) NumExpr :: subst(std::string val, PTR(Expr) e) {
//    return this;
//}
//
//PTR(Expr)AddExpr:: subst(std::string var, PTR(Expr) new_expression) {
//    return NEW (AddExpr)((this->lhs)->subst(var, new_expression), (this->rhs)->subst(var, new_expression));
//}
//
//PTR(Expr) MultExpr :: subst(std::string var, PTR(Expr) new_expression) {
//    return NEW (MultExpr)((this->lhs)->subst(var, new_expression), (this->rhs)->subst(var, new_expression));
//}
//
//PTR(Expr) VarExpr :: subst(std::string var, PTR(Expr) new_expression) {
//
//    if(this->name == var) {
//        return new_expression;
//
//    }
//    else return this;
//
//}
//
//PTR(Expr) LetExpr :: subst(std::string var, PTR(Expr) new_expression) {
//    if (var == this->lhs) {
//        return NEW (LetExpr)(this->lhs,this->rhs->subst(var, new_expression), this->body);
//    }
//    return NEW (LetExpr)(this->lhs, this->rhs->subst(var, new_expression), this->body->subst(var, new_expression));
//}
//
//PTR(Expr)BoolExpr:: subst(std::string var, PTR(Expr) e) {
//    return this;
//}
//
//
//PTR(Expr) IfExpr :: subst(std:: string var,PTR(Expr) e) {
//    PTR(Expr) new_test;
//    PTR(Expr) new_then;
//    PTR(Expr)new_else;
//
//    new_test = this->test->subst(var, e);
//    new_then = this->then->subst(var, e);
//    new_else = this->else_->subst(var, e);
//
//    return NEW (IfExpr)(new_test, new_then, new_else);
//}
//
//PTR(Expr) EqExpr :: subst(std::string var, PTR(Expr) e) {
//    PTR(Expr) new_rhs;
//    PTR(Expr) new_lhs;
//
//    new_rhs = this->rhs->subst(var, e);
//    new_lhs = this->lhs->subst(var, e);
//
//    EqExpr*new_eq = NEW (EqExpr)(new_lhs, new_rhs);
//    return new_eq;
//}
//
//PTR(Expr) FunExpr :: subst(std::string var, PTR(Expr) e) {
//    return NEW (FunExpr)(this->formal_arg, this->body->subst(var, e));
//
//}
//
//PTR(Expr) CallExpr :: subst(std::string var, PTR(Expr) e) {
//    return NEW (CallExpr)(this->actual_arg, this->subst(var, e));
//}


/*//////////////////////PRINT METHOD/////////////////////////////////////////////////////////////////////////////////*/


void NumExpr :: print(std::ostream& output) {
    output << this-> rep;
}

void AddExpr :: print(std::ostream &output) {
    PTR(Expr) new_lhs = this -> lhs;
    PTR(Expr) new_rhs = this -> rhs;
    
    output<< "(";
    new_lhs-> print(output);
    output<< "+";
    new_rhs-> print(output);
    output<< ")";
    
}

void MultExpr:: print(std::ostream& output) {
    PTR(Expr) new_lhs = this -> lhs;
    PTR(Expr) new_rhs = this -> rhs;
    
    output<< "(";
    new_lhs-> print(output);
    output<< "*";
    new_rhs-> print(output);
    output<< ")";
}

void VarExpr :: print(std::ostream& output) {
    output<< this-> name;
}

void LetExpr :: print(std::ostream& output) {
    output << "(_let ";
    output << this->lhs;
    output << "=";
    this->rhs->print(output);
    output << " _in ";
    this->body->print(output);
    output << ")";
    
}


void BoolExpr::print(std::ostream& output) {
    if(this->val == true) {
        output << "_true";
    }
    else{
        output << "_false";
    }
}

void IfExpr::print(std::ostream& output) {
    PTR(Expr) new_test = this->test;
    PTR(Expr) new_then = this->then;
    PTR(Expr) new_else= this-> else_;
    
    output << "(_if ";
    new_test->print(output);
    output << " _then ";
    new_then->print(output);
    output << " _else ";
    new_else->print(output);
    output << ")";
}

void EqExpr::print(std::ostream& output) {
    PTR(Expr) new_lhs = this->lhs;
    PTR(Expr) new_rhs = this->rhs;
    
    output << "(";
    new_lhs->print(output);
    output << "==";
    new_rhs->print(output);
    output << ")";
}

void FunExpr::print(std::ostream& output) {
    output<< "(_fun (" << formal_arg << ") ";
    this->body->print(output);
    output<< ")";
}

void CallExpr::print(std::ostream& output) {
    this->to_be_called->print(output);
    output << "(";
    this->actual_arg->print(output);
    output << ")";
}


/*///////////////////PRETTY PRINT AT METHOD////////////////////////////////////////////////////////////////////////////*/

void NumExpr ::pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside) {
    type = print_none;
    inside = 0;
    output << this-> rep;
}

void AddExpr::pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside){
    inside += 2;
    PTR(Expr) new_lhs = this -> lhs;
    PTR(Expr) new_rhs = this -> rhs;
    if (type >= print_add){
        output << "(";
    }
    new_lhs->pretty_print_at(output, print_add, indentation, inside);
    output << " + ";
    new_rhs->pretty_print_at(output, print_none, indentation, inside);
        if(type >= print_add){
    output << ")";
    }
  
}

void MultExpr::pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside){
    PTR(Expr)new_lhs = this -> lhs;
    PTR(Expr) new_rhs = this -> rhs;
    inside += 1;
    
    if(type >= print_group_add_mult) {
        output<< "(";
    }
        new_lhs->pretty_print_at(output, print_group_add_mult, indentation, inside);
        output << " * ";
        new_rhs->pretty_print_at(output, print_add, indentation, inside);
    if(type >= print_group_add_mult) {
        output << ")";
    }
        
  
}


void VarExpr::pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside){
    type = print_none;
    inside = 0;
    output << this->name;
}


void LetExpr :: pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside) {
   
    if(type > print_none && inside >= 1){
        output << "(" ;
       
    }
    int startPos = (int) output.tellp();
    output << "_let " << this->lhs << " = ";
       this->rhs->pretty_print_at(output, print_none, indentation, inside);
       output << "\n";
       int pos2 = (int) output.tellp();
       int n = startPos - indentation;
       output << std::string(n, ' ') << "_in  ";
       this->body->pretty_print_at(output, print_none, pos2, inside);
       if (type > print_none && inside >= 1) {
           output << ")";
       }
    
}


void BoolExpr::pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside){
    print(output);
}


void IfExpr :: pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside) {
    if (type > print_none && inside >= 1) {
         output << "(";
     }
     int startPos = (int) output.tellp();
     output << "_if ";
     this->test->pretty_print(output);
     output << "\n";
     int pos2 = (int) output.tellp();
     int n = startPos - indentation;
     output << std::string(n, ' ') << "_then ";
     this->then->pretty_print_at(output, print_none, pos2, inside);
     output << "\n";
     pos2 = (int) output.tellp();
     output << std::string(n, ' ') << "_else ";
     this->else_->pretty_print_at(output, print_none, pos2, inside);
     if (type > print_none && inside >= 1) {
         output << ")";
     }
    
}



void EqExpr::pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside) {
    inside += 2;
       if (type >= print_add) {
           output << "(";
       }
       this->lhs->pretty_print_at(output, print_add, indentation, inside);
       output << " == ";
       this->rhs->pretty_print_at(output, print_none, indentation, 0);
       if (type >= print_add) {
           output << ")";
       }
}

void FunExpr::pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside) {
    if(type > print_none && inside >= 1) {
        output << "(";
    }
    
    int startPos = (int) output.tellp();
    output << "_fun (";
    output << formal_arg;
    output << ")\n";
    int pos2 = (int) output.tellp();
    int n = startPos - indentation;
    output << std::string(n, ' ') << "  ";
    this->body->pretty_print_at(output, print_none, pos2, inside);
    
    if(type > print_none && inside >= 1) {
        output << ")";
    }
}

void CallExpr::pretty_print_at(std::ostream& output, print_mode_t type, int indentation, int inside) {

    this->to_be_called->pretty_print_at(output,print_group_add_mult, indentation, inside);
    output << "(";
    this->actual_arg->pretty_print_at(output,print_none, indentation, inside);
    output << ")";

}


/*///////////HAS VARIABLE METHOD----VOID////////////////////////////////////////////////////////////////////////////*/
//
//bool NumExpr :: has_variable() {
//    return false;
//}
//
//bool AddExpr:: has_variable() {
//    return lhs->has_variable() || rhs->has_variable();
//}
//
//
//bool MultExpr:: has_variable() {
//    return lhs->has_variable() || rhs->has_variable();
//}
//
//
//bool VarExpr:: has_variable() {
//    return true;
//}
//
//bool LetExpr :: has_variable(){
//    return rhs->has_variable() || body->has_variable();
//}
//
//
//bool BoolExpr::has_variable(){
//    return false;
//}
//
//
//bool IfExpr::has_variable() {
//    return (this->test->has_variable() || this->then->has_variable() || this->else_->has_variable());
//}
//
//
//bool EqExpr::has_variable(){
//    return (this->lhs->has_variable() || this->rhs->has_variable());
//}


/*//////////TEST CASES////////////////////////////////////////////////////////////////////////////////////////////*/


    


