% Hechos: compra(Comprador, Producto).
compra(juan, manzana).
compra(maria, manzana).
compra(pedro, pera).
compra(ana, manzana).
compra(carlos, pera).
compra(ana, uva).
compra(pedro, manzana).
compra(pedro, mango).


precio(manzana, 35).
precio(uva, 48).
precio(pera, 32).
precio(mango, 49).

% contar las compras de un producto
veces_vendido(Producto, N) :-
    findall(Producto, compra(_, Producto), Lista),
    length(Lista, N).


total_producto(Producto, Total) :-
    findall(Precio, (compra(_, Producto), precio(Producto, Precio)), Lista),
    sum_list(Lista, Total).


total_cliente(Cliente, Total) :-
    findall(Precio, (compra(Cliente, Producto), precio(Producto, Precio)), Lista),
    sum_list(Lista, Total).


articulos_cliente(Cliente, Lista) :-
    findall(Producto, compra(Cliente, Producto), Lista).


