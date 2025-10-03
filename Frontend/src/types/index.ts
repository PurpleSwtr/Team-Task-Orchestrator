export interface MenuItem {
  message: string;
  icon: string;
  route_path?: string; 
}

export interface Task {
  id: number;
  tittle: string;
  text_task: string;
}