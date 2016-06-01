# -*- mode: ruby -*-
# vi: set ft=ruby :

Vagrant.configure(2) do |config|

	config.vm.box = "ubuntu/trusty64"
	config.vm.network "forwarded_port", guest: 5672, host: 5672
	config.vm.network "forwarded_port", guest: 4242, host: 4242
	config.vm.network "forwarded_port", guest:15672, host: 15672
	config.vm.provision "shell", inline: <<-SHELL
		sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
		echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
		sudo apt-get update
		sudo apt-get -y install mono-devel
		sudo apt-get -y install mono-complete 
		sudo apt-get -y install libevent-core-2.0-5 libevent-extra-2.0-5 libevent-pthreads-2.0-5

		echo 'deb http://www.rabbitmq.com/debian/ testing main' | sudo tee /etc/apt/sources.list.d/rabbitmq.list
		wget -O- https://www.rabbitmq.com/rabbitmq-release-signing-key.asc | sudo apt-key add -
		sudo apt-get update
		sudo apt-get -y install rabbitmq-server
		sudo rabbitmq-plugins enable rabbitmq_management
		sudo service rabbitmq-server restart
		sudo rabbitmqctl add_user admin admin
		sudo rabbitmqctl set_user_tags admin administrator
		sudo rabbitmqctl set_permissions -p / admin ".*" ".*" ".*"
	SHELL
end
