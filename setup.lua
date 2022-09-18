function setup()
	vendor = {
		name = "vendor",
		strength = 22,
		dexterity = 22,
		iq = 22,
		armour = 20,
		lowest = 30,
		highest = 0
	}

	mouse = {
		name = "mouse",
		strength = 6,
		dexterity = 8,
		iq = 12,
		armour = 4,
		lowest = 15,
		highest = 0
	}

	rat = {
		name = "rat",
		strength = 8,
		dexterity = 10,
		iq = 12,
		armour = 6,
		lowest = 20,
		highest = 0
	}

	dog = {
		name = "dog",
		strength = 10,
		dexterity = 12,
		iq = 14,
		armour = 10,
		lowest = 20,
		highest = 0
	}

	vampire = {
		name = "vampire",
		strength = 12,
		dexterity = 16,
		iq = 18,
		armour = 12,
		lowest = 30,
		highest = 20
	}

	mimic = {
		name = "mimic",
		strength = 20,
		dexterity = 12,
		iq = 12,
		armour = 10,
		lowest = 40,
		highest = 20
	}

	wizard = {
		name = "wizard",
		strength = 12,
		dexteity = 16,
		iq = 22,
		armour = 12,
		lowest = 60,
		highest = 20
	}

	dagger = {
		name = "dagger",
		damage = 4
	}

	sword = {
		name = "sword",
		damage = 6
	}

	monsters = {
		vendor,
		mouse,
		rat,
		dog,
		vampire,
		mimic,
		wizard
	}

	weapons = {
		dagger,
		sword
	}

	return monsters, weapons
end

monsters, weapons = setup()

