param
(
    [switch]$DropVolume = $false
)

if($DropVolume)
{
	docker-compose down -v;
}
else 
{
	docker-compose down;
}