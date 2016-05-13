 module MyAndromeda.Menu.Services 
 {
    export class CategoryService {
        constructor(public categories: Models.ICategory[]) { }

        public findById(id: number): Models.ICategory {
            for (var i = 0; i < this.categories.length; i++) {
                var c = this.categories[i];
                if (c.Id === id) { return c; }
            }
            return null;
        }

        public findCategoriesByParentId(id: number): Models.ICategory[] {
            var elements = $.grep(this.categories, (category, index) => {
                return category.ParentId === id;
            });

            return elements;
        }
    }
 }