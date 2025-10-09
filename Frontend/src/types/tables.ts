import type { Component } from "vue";

export interface UserData {
  id: string;
  gender: string;
  // firstName: string;
  // secondName: string;
  // lastName: string | null; 
  shortName: string;
  roles: Array<[]>;
  email: string | null; 
}


export interface UserDataForAdmin {
  id: string;
  gender: string;
  // firstName: string;
  // secondName: string;
  // lastName: string | null; 
  shortName: string;
  roles: Array<[]>;
  email: string | null; 
}