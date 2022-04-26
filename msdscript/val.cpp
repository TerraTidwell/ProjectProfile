//
//  val.cpp
//  msdscript
//
//  Created by Terra Tidwell on 2/14/22.
//

#include "val.hpp"
#include "Expr.hpp"
#include <sstream>


std::string Val::to_string(){
    std::ostream stream(nullptr);
    std::stringbuf str;
    stream.rdbuf(&str);
    print(stream);
    return str.str();
}

/*////////////////////////////NUM CLASS/////////////////////////////////////////////////////////////////////////*/
NumVal::NumVal(int val){
    this->val = val;
}

bool NumVal::equals(PTR(Val) lhs){
    PTR(NumVal) other_num = CAST(NumVal)(lhs);
    if (other_num == NULL){
        return false;
    } else {
        return (this->val == other_num->val);
    }
}

PTR(Val) NumVal::add_to(PTR(Val)lhs){
    PTR(NumVal) other_num = CAST(NumVal)(lhs);
    if (other_num == NULL) throw std::runtime_error("addition of non-number");
    return NEW (NumVal)(this->val + other_num->val);
}

PTR(Val) NumVal::mult_to(PTR(Val)lhs){
    PTR(NumVal)other_num = CAST(NumVal)(lhs);
    if (other_num == NULL) throw std::runtime_error("multiplication of non-number");
    return NEW (NumVal)(this->val * other_num->val);
}

void NumVal::print(std::ostream& outstream){
    outstream << (this->val);
}
PTR(Expr) NumVal :: to_expr() {
    return NEW (NumExpr)(val);
}

bool NumVal :: is_true(){
    throw std::runtime_error("Test expression is not a boolean");
}

PTR(Val) NumVal::call(PTR(Val) actual_arg) {
    throw std::runtime_error("Not a function to be called error");
}
std::string NumVal::to_string() {
    return std::to_string(val);
}
void NumVal::call_step(PTR(Val) actual_arg_val, PTR(Cont) rest) {
    throw std::runtime_error("NumVal call error");
}

/*/////////BOOL CLASS//////////////////////////////////////////////////////////////////////////////////////////*/
BoolVal :: BoolVal(bool val) {
    this->val = val;
}

PTR(Expr) BoolVal:: to_expr(){
    return NEW (BoolExpr)(this-> val);
}

bool BoolVal::equals(PTR(Val) v) {
    PTR(BoolVal) other = CAST(BoolVal)(v); \
    
    if (other == NULL) {
        return false;
    }
    else {
        return (this->val == other->val);
    }
}
PTR(Val)BoolVal :: add_to(PTR(Val) rhs) {
    throw std::runtime_error("addition of boolean");
    
}

PTR(Val) BoolVal :: mult_to(PTR(Val)rhs) {
    throw std:: runtime_error("multiplication of boolean");
}

void BoolVal::print(std::ostream& output) {
    if (this->val == true) {
        output << "_true";
    }else {
        output<< "_false";
    }
}

bool BoolVal::is_true() {
    return this->val;
}

PTR(Val) BoolVal::call(PTR(Val) actual_arg) {
    throw std::runtime_error("Not a function to be called error");
}
std::string BoolVal::to_string() {
    if (this->val == true) {
        return "_true";
    }
    else {
        return "_false";
    }
}
void BoolVal::call_step(PTR(Val) actual_arg_val, PTR(Cont) rest) {
    throw std::runtime_error("BoolVal call error");
}

/*////////////////////////FUN CLASS////////////////////////////////////////////////////////////////////////////*/

FunVal::FunVal(std::string formal_arg, PTR(Expr) body, PTR(Env) env) {
    this->formal_arg = formal_arg;
    this->body = body;
    this->env = env;
}


bool FunVal::equals(PTR(Val) v) {
    PTR(FunVal) other_val = CAST(FunVal)(v);
    if (other_val == NULL) {
        return false;
    } else {
        return ((this->formal_arg) == (other_val->formal_arg)) && ((this->body) ->equals (other_val-> body)) && ((this->env)->equals (other_val -> env));
    }
}


PTR(Expr) FunVal :: to_expr(){
    return NEW (FunExpr)(this->formal_arg, this->body);
}


PTR(Val) FunVal:: add_to(PTR(Val) rhs) {
    throw std::runtime_error("addition of non-number");
}

PTR(Val) FunVal:: mult_to(PTR(Val) rhs) {
    throw std::runtime_error("multiplication of non-number");
}

void FunVal::print(std::ostream& output) {
    output << "(_fun (" << formal_arg << ") ";
    this-> body->print(output);
    output << ")";
}

bool FunVal::is_true() {
    throw std::runtime_error("not a boolean");
}
PTR(Val) FunVal::call(PTR(Val) actual_arg) {
    return this->body->interp(NEW(ExtendedEnv)(formal_arg, actual_arg, env));
}
std::string FunVal::to_string() {
    return "[function]";
}
void FunVal::call_step(PTR(Val) actual_arg_val, PTR(Cont) rest) {
    Step::mode = Step::interp_mode;
    Step::expr = body;
    Step::env = NEW(ExtendedEnv)(formal_arg, actual_arg_val, env);
    Step::cont = rest;
}
