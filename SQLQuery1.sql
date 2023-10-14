 INSERT INTO Fruits_Vegetables_t (Name_f, Type_f, Color_f, Calorie_f) VALUES ('Apple', 'Fruit', 'Red', 150);

 

 update Fruits_Vegetables_t set  Name_f=2,Type_f=3,Color_f=5,Calorie_f=6
 where id=5

 delete from Fruits_Vegetables_t
 where id=2

select avg(Calorie_f)
from Fruits_Vegetables_t
select count(*) from Fruits_Vegetables_t where Type_f like 'vegetable' or Type_f like 'Vegetable'
select * from Fruits_Vegetables_t
order by id

 select DISTINCT Name_f from Fruits_Vegetables_t

