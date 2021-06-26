�
gC:\Users\ngrus\Google Drive\Repos\ngruson\AdventureWorks\src\AW.Common\AutoMapper\BaseMappingProfile.cs
	namespace 	
AW
 
. 
Common
. 

AutoMapper 
{ 
public 

class 
BaseMappingProfile #
:$ %
Profile& -
{		 
public

 
BaseMappingProfile

 !
(

! "
)

" #
{ 	%
ApplyMappingsFromAssembly %
(% &
Assembly& .
.. / 
GetExecutingAssembly/ C
(C D
)D E
)E F
;F G
}
	protected 
void %
ApplyMappingsFromAssembly 0
(0 1
Assembly1 9
assembly: B
)B C
{ 	
var 
types 
= 
assembly  
.  !
GetExportedTypes! 1
(1 2
)2 3
. 
Where 
( 
t 
=> 
t 
. 

(+ ,
), -
.- .
Any. 1
(1 2
i2 3
=>4 6
i 
. 

&&$ &
i' (
.( )$
GetGenericTypeDefinition) A
(A B
)B C
==D F
typeofG M
(M N
IMapFromN V
<V W
>W X
)X Y
)Y Z
) 
. 
ToList 
( 
) 
; 
var 

= 
typeof  &
(& '
IMapFrom' /
</ 0
>0 1
)1 2
;2 3
var 

methodName 
= 
nameof #
(# $
IMapFrom$ ,
<, -
object- 3
>3 4
.4 5
Mapping5 <
)< =
;= >
var 

= 
new  #
Type$ (
[( )
]) *
{+ ,
typeof- 3
(3 4
Profile4 ;
); <
}= >
;> ?
foreach 
( 
var 
type 
in  
types! &
)& '
{ 
var 

interfaces 
=  
type! %
.% &

(3 4
)4 5
. 
Where 
( 
i 
=> 
i  !
.! "

&&0 2
i3 4
.4 5$
GetGenericTypeDefinition5 M
(M N
)N O
==P R

)` a
. 
ToList 
( 
) 
; 
if"" 
("" 

interfaces"" 
."" 
Count"" $
>""% &
$num""' (
)""( )
{## 
var&& 
instance&&  
=&&! "
	Activator&&# ,
.&&, -
CreateInstance&&- ;
(&&; <
type&&< @
)&&@ A
;&&A B
foreach)) 
()) 
var))  
i))! "
in))# %

interfaces))& 0
)))0 1
{** 
var++ 

methodInfo++ &
=++' (
i++) *
.++* +
	GetMethod+++ 4
(++4 5

methodName++5 ?
,++? @

)++N O
;++O P

methodInfo,, "
?,," #
.,,# $
Invoke,,$ *
(,,* +
instance,,+ 3
,,,3 4
new,,5 8
object,,9 ?
[,,? @
],,@ A
{,,B C
this,,D H
},,I J
),,J K
;,,K L
}-- 
}.. 
}// 
}00 	
}11 
}22 �
]C:\Users\ngrus\Google Drive\Repos\ngruson\AdventureWorks\src\AW.Common\AutoMapper\IMapFrom.cs
	namespace 	
AW
 
. 
Common
. 

AutoMapper 
{ 
public 

	interface 
IMapFrom 
< 
T 
>  
{ 
void

 
Mapping


(

 
Profile

 
profile

 $
)

$ %
=>

& (
profile

) 0
.

0 1
	CreateMap

1 :
(

: ;
typeof

; A
(

A B
T

B C
)

C D
,

D E
GetType

F M
(

M N
)

N O
)

O P
;

P Q
} 
}

aC:\Users\ngrus\Google Drive\Repos\ngruson\AdventureWorks\src\AW.Common\Extensions\GuardClauses.cs
	namespace 	
AW
 
. 
Common
. 

Extensions 
{ 
public 

static 
class 
GuardClauses $
{ 
public		 
static		 
T		 
Null		 
<		 
T		 
>		 
(		  
this		  $
IGuardClause		% 1
guardClause		2 =
,		= >
T		? @
input		A F
,		F G
string		H N

,		\ ]
ILogger		^ e
logger		f l
)		l m
{

 	
if 
( 
input 
is 
null 
) 
{ 
var
ex
=
new
ArgumentNullException
(

)
;
logger 
. 
LogError 
(  
ex  "
," #
ex$ &
.& '
Message' .
). /
;/ 0
throw 
ex 
; 
} 
return 
input 
; 
} 	
} 
} �
eC:\Users\ngrus\Google Drive\Repos\ngruson\AdventureWorks\src\AW.Common\Extensions\PersonExtensions.cs
	namespace 	
AW
 
. 
Common
. 

Extensions 
{ 
public 

static 
class 
PersonExtensions (
{ 
public 
static 
string 
FullName %
(% &
this& *
IPerson+ 2
person3 9
)9 :
{ 	
string		 
fullName		 
=		 
$str		  
;		  !
if 
( 
! 
string 
. 

(% &
person& ,
., -
	FirstName- 6
)6 7
)7 8
fullName 
= 
person !
.! "
	FirstName" +
;+ ,
if 
( 
! 
string 
. 

(% &
person& ,
., -

MiddleName- 7
)7 8
)8 9
fullName 
+= 
$" 
$str 
{  
person  &
.& '

MiddleName' 1
}1 2
"2 3
;3 4
if 
( 
! 
string 
. 

(% &
person& ,
., -
LastName- 5
)5 6
)6 7
fullName 
+= 
$" 
$str 
{  
person  &
.& '
LastName' /
}/ 0
"0 1
;1 2
return 
fullName 
; 
} 	
} 
} �
\C:\Users\ngrus\Google Drive\Repos\ngruson\AdventureWorks\src\AW.Common\Interfaces\IPerson.cs
	namespace 	
AW
 
. 
Common
. 

Interfaces 
{ 
public 

	interface 
IPerson 
{ 
string 
	FirstName 
{ 
get 
; 
set  #
;# $
}% &
string 

MiddleName 
{ 
get 
;  
set! $
;$ %
}& '
string 
LastName 
{ 
get 
; 
set "
;" #
}$ %
} 
}		 