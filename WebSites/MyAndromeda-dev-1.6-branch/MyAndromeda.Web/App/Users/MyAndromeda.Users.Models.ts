module MyAndromeda.Users.Models {
    export class User implements IUser {
        public Id: number;
        public FirstName: string;
        public SurName: string;
        public Username: string;
        public Roles: IRole[];

        constructor(id: number, userName: string, firstName: string, surName: string, roles: IRole[])
        {
            this.Id = id;
            this.Username = userName;
            this.FirstName = firstName;
            this.SurName = surName;
            if (roles) 
            { 
                this.Roles = roles; 
            }
        }
    }
}


